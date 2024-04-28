using CodeOrderAPI.Data;
using CodeOrderAPI.Model;
using CodeOrderAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CodeOrderAPI.Routes;

    public static class PlanetaRoute
    {
        public static void MapPlanetaEndpoints(this WebApplication app)
        {
        app.MapGet("/Planeta", async (DataContext context, CancellationToken cancellationToken) =>
        {
            var planets = await context
                .Planetas
                .Select(planet =>
                    new
                    {
                        planet.Id,
                        planet.Name,
                        planet.RotationPeriod,
                        planet.OrbitalPeriod,
                        planet.Diameter,
                        planet.Climate,
                        planet.Gravity,
                        planet.Terrain,
                        planet.SurfaceWater,
                        planet.Population,
                        Characters = planet.Characters.Select(character =>
                            new
                            {
                                character.Id,
                                character.Name,
                            }),
                        Movies = planet.Movies.Select(movie =>
                            new
                            {
                                movie.Id,
                                movie.Title,
                                movie.Episode,
                            }),
                    })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (planets.Any())
                return Results.Ok(planets);

            return Results.NoContent();
        });

        app.MapGet("/Planeta/{id}", async (
            [FromRoute] int id,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var planet = await context
                .Planetas
                .Where(planet => planet.Id == id)
                .Select(planet =>
                    new
                    {
                        planet.Id,
                        planet.Name,
                        planet.RotationPeriod,
                        planet.OrbitalPeriod,
                        planet.Diameter,
                        planet.Climate,
                        planet.Gravity,
                        planet.Terrain,
                        planet.SurfaceWater,
                        planet.Population,
                        Characters = planet.Characters.Select(character =>
                            new
                            {
                                character.Id,
                                character.Name,
                            }),
                        Movies = planet.Movies.Select(movie =>
                            new
                            {
                                movie.Id,
                                movie.Title,
                                movie.Episode,
                            }),
                    })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (planet is null)
                return Results.NoContent();

            return Results.Ok(planet);
        });

        app.MapDelete("/Planeta/{id}", async (
            [FromRoute] int id,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var modelFound = await context
                .Planetas
                .FirstOrDefaultAsync(planet => planet.Id == id, cancellationToken);

            if (modelFound is null)
                return Results.NotFound();

            context.Planetas.Remove(modelFound);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok(modelFound);
        });

        app.MapPost("/Planeta", async (
            [FromBody] PlanetToAddViewModel modelToAdd,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var modelAlreadyAdded = await context.Planetas.FirstOrDefaultAsync(
                planet => planet.Name == modelToAdd.Name,
                cancellationToken);

            if (modelAlreadyAdded is not null)
                return Results.Conflict("planet name already registered.");

            var planet = new Planeta
            {
                RotationPeriod = modelToAdd.RotationPeriod,
                OrbitalPeriod = modelToAdd.OrbitalPeriod,
                Diameter = modelToAdd.Diameter,
                Climate = modelToAdd.Climate,
                Gravity = modelToAdd.Gravity,
                Terrain = modelToAdd.Terrain,
                SurfaceWater = modelToAdd.SurfaceWater,
                Population = modelToAdd.Population
            };

            var charactersRelationResult
                = await GetCharactersByIdsAsync(context, modelToAdd.CharactersIds.ToArray(), cancellationToken);

            if (charactersRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(charactersRelationResult.GetErrorMessage("Characters"));

            planet.Characters.AddRange(charactersRelationResult.Entities);

            var moviesRelationResult
                = await GetMoviesByIdsAsync(context, modelToAdd.MoviesIds.ToArray(), cancellationToken);

            if (moviesRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(moviesRelationResult.GetErrorMessage("Movies"));

            planet.Characters.AddRange(charactersRelationResult.Entities);

            var transaction =
                await context.Database.BeginTransactionAsync(cancellationToken);

            var modelAddedResult = await context.Planetas.AddAsync(planet);

            await transaction.CommitAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Created("/Planeta/{id}", modelAddedResult.Entity.Id);
        });

        app.MapPut("/Planeta/{id}", async (
            [FromRoute] int id,
            [FromBody] PlanetToUpdateViewModel modelToUpdate,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var planet = await context.Planetas.FirstOrDefaultAsync(
                planet => planet.Id == id,
                cancellationToken);

            if (planet is null)
                return Results.NotFound("planet was not found.");

            var nameAlreadyAdded =
                planet.Name != modelToUpdate.Name
                 ? (await context.Planetas.FirstOrDefaultAsync(
                 planet => planet.Name == modelToUpdate.Name,
                    cancellationToken))
                    is not null
                : false;

            if (nameAlreadyAdded)
                return Results.Conflict("planet title already registered.");

            var charactersRelationResult
                = await GetCharactersByIdsAsync(context, modelToUpdate.CharactersIdsToReplace.ToArray(), cancellationToken);

            if (charactersRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(charactersRelationResult.GetErrorMessage("Characters"));

            var moviesRelationResult
                = await GetMoviesByIdsAsync(context, modelToUpdate.MoviesIdsToReplace.ToArray(), cancellationToken);

            if (moviesRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(moviesRelationResult.GetErrorMessage("Movies"));


            var transaction =
                await context.Database.BeginTransactionAsync(cancellationToken);

            planet.RotationPeriod = modelToUpdate.RotationPeriod;
            planet.OrbitalPeriod = modelToUpdate.OrbitalPeriod;
            planet.Diameter = modelToUpdate.Diameter;
            planet.Climate = modelToUpdate.Climate;
            planet.Gravity = modelToUpdate.Gravity;
            planet.Terrain = modelToUpdate.Terrain;
            planet.SurfaceWater = modelToUpdate.SurfaceWater;
            planet.Population = modelToUpdate.Population;

            //
            // Deleting every character and movie to add after
            //
            (await context.Planetas.Include(f => f.Characters).SingleAsync(f => f.Id == planet.Id)).Characters.Clear();
            (await context.Planetas.Include(f => f.Movies).SingleAsync(f => f.Id == planet.Id)).Movies.Clear();


            planet.Characters.AddRange(charactersRelationResult.Entities);

            planet.Movies.AddRange(moviesRelationResult.Entities);

            await transaction.CommitAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok(id);
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
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return new(
            IdsDidntMatch: ids.Where(id => !characters.Any(c => c.Id == id)).ToArray(),
            Entities: characters);
    }

    private static async Task<RelationResult<Filme>> GetMoviesByIdsAsync(
    DataContext context,
    int[] ids,
    CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            return RelationResult<Filme>.Empty;

        var movies = await context
            .Filmes
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return new(
            IdsDidntMatch: ids.Where(id => !movies.Any(c => c.Id == id)).ToArray(),
            Entities: movies);
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
