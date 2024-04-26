using CodeOrderAPI.Data;
using CodeOrderAPI.Model;
using CodeOrderAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CodeOrderAPI.Routes;

public static class FilmeRoute
{
    public static void MapFilmeEndpoints(this WebApplication app)
    {
        app.MapGet("/Filme", (DataContext context, CancellationToken cancellationToken) =>
        {
            return context
                .Filmes
                .Select(movie =>
                    new
                    {
                        movie.Title,
                        movie.Episode,
                        movie.OpeningCrawl,
                        movie.Director,
                        movie.Producer,
                        movie.ReleaseDate,
                        Characters = movie.Characters.Select(character =>
                            new 
                            { 
                                character.Id,
                                character.Name,
                            }),
                        Planets = movie.Planets.Select(planet => 
                            new
                            {
                                planet.Id,
                                planet.Name,
                            }),
                        Vehicles = movie.Veichles.Select(vehicle => 
                            new
                            {
                                vehicle.Id, 
                                vehicle.Name,
                            }),
                        Starships = movie.Starships.Select(starship =>
                            new
                            {
                                starship.Id,
                                starship.Name,
                            })
                    })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        });

        app.MapGet("/Filme/{id}", (
            [FromRoute]int id, 
            DataContext context, 
            CancellationToken cancellationToken) =>
        {
            return context
                .Filmes
                .Where(movie => movie.Id == id)
                .Select(movie =>
                    new
                    {
                        movie.Title,
                        movie.Episode,
                        movie.OpeningCrawl,
                        movie.Director,
                        movie.Producer,
                        movie.ReleaseDate,
                        Characters = movie.Characters.Select(character =>
                            new
                            {
                                character.Id,
                                character.Name,
                            }),
                        Planets = movie.Planets.Select(planet =>
                            new
                            {
                                planet.Id,
                                planet.Name,
                            }),
                        Vehicles = movie.Veichles.Select(vehicle =>
                            new
                            {
                                vehicle.Id,
                                vehicle.Name,
                            }),
                        Starships = movie.Starships.Select(starship =>
                            new
                            {
                                starship.Id,
                                starship.Name,
                            })
                    })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        });

        app.MapDelete("/Filme/{id}", async (
            [FromRoute] int id,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var modelFound = await context
                .Filmes
                .FirstOrDefaultAsync(movie => movie.Id == id, cancellationToken);

            if (modelFound is null)
                return Results.NotFound();

            context.Filmes.Remove(modelFound);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok(modelFound);
        });

        app.MapPost("/Filme", async (
            [FromBody] MovieToAddViewModel modelToAdd,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var modelAlreadyAdded = await context.Filmes.FirstOrDefaultAsync(
                movie => movie.Title == modelToAdd.Title,
                cancellationToken);

            if (modelAlreadyAdded is not null)
                return Results.Conflict("Já existe filme com este nome.");

            var movie = new Model.Filme
            {
                Director = modelToAdd.Director,
                Episode = modelToAdd.Episode,
                OpeningCrawl = modelToAdd.OpeningCrawl,
                Producer = modelToAdd.Producer,
                Title = modelToAdd.Title,
                ReleaseDate = modelToAdd.ReleaseDate
            };

            var vehiclesRelationResult
                = await GetVehiclesByIdsAsync(context, modelToAdd.CharactersIds.ToArray(), cancellationToken);

            if (vehiclesRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(vehiclesRelationResult.GetErrorMessage("Vehicles"));

            movie.Veichles.AddRange(vehiclesRelationResult.Entities);

            var charactersRelationResult
                = await GetCharactersByIdsAsync(context, modelToAdd.CharactersIds.ToArray(), cancellationToken);

            if (charactersRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(charactersRelationResult.GetErrorMessage("Characters"));

            movie.Characters.AddRange(charactersRelationResult.Entities);

            var starshipsRelationResult
                = await GetStarshipsByIdsAsync(context, modelToAdd.StarshipsIds.ToArray(), cancellationToken);

            if (starshipsRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(starshipsRelationResult.GetErrorMessage("Starships"));

            movie.Starships.AddRange(starshipsRelationResult.Entities);

            var planetsRelationResult 
                = await GetPlanetsByIdsAsync(context, modelToAdd.PlanetsIds.ToArray(), cancellationToken);

            if (planetsRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(planetsRelationResult.GetErrorMessage("Planets"));

            movie.Planets.AddRange(planetsRelationResult.Entities);

            var transaction = 
                await context.Database.BeginTransactionAsync(cancellationToken);

            var modelAddedResult = await context.Filmes.AddAsync(movie);

            await transaction.CommitAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Created("/Filme/{id}", modelAddedResult.Entity.Id);
        });
    }

    private static async Task<RelationResult<Personagem>> GetCharactersByIdsAsync(
        DataContext context,
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            return RelationResult<Personagem>.Empty;

        var characters = await context
            .Personagens
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return new(
            IdsDidntMatch: ids.Where(id => !characters.Any(c => c.Id == id)).ToArray(),
            Entities: characters);
    }

    private static async Task<RelationResult<Nave>> GetStarshipsByIdsAsync(
        DataContext context,
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            return RelationResult<Nave>.Empty;

        var starships = await context
            .Naves
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return new(
            IdsDidntMatch: ids.Where(id => !starships.Any(s => s.Id == id)).ToArray(),
            Entities: starships);
    }

    private static async Task<RelationResult<Planeta>> GetPlanetsByIdsAsync(
        DataContext context,
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            return RelationResult<Planeta>.Empty;

        var planets = await context
            .Planetas
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return new(
            IdsDidntMatch: ids.Where(id => !planets.Any(p => p.Id == id)).ToArray(),
            Entities: planets);
    }

    private static async Task<RelationResult<Veiculo>> GetVehiclesByIdsAsync(
        DataContext context,
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            return RelationResult<Veiculo>.Empty;

        var veiculos = await context
            .Veiculos
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return new(
            IdsDidntMatch: ids.Where(id => !veiculos.Any(v => v.Id == id)).ToArray(),
            Entities: veiculos);
    }

    private record RelationResult<TEntity>(
        int[] IdsDidntMatch,
        IEnumerable<TEntity> Entities)
    {
        public static readonly RelationResult<TEntity> Empty
            = new(Array.Empty<int>(), Enumerable.Empty<TEntity>());

        public bool ContainsIdsDidntMatch => IdsDidntMatch.Length > 0;

        public string GetErrorMessage(string fieldName)
        {
            return $"{fieldName} ids '{string.Join(", ", IdsDidntMatch)}' did not match.";
        }
    }
}
