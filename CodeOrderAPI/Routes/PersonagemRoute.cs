using CodeOrderAPI.Data;
using CodeOrderAPI.Model;
using CodeOrderAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace CodeOrderAPI.Routes;

public static class PersonagemRoute
{
    public static void MapPersonagemEndpoints(this WebApplication app)
    {
        app.MapGet("/Personagem", async (DataContext context, CancellationToken cancellationToken) =>
        {
            var planet = await context
                .Personagens
                .Select(character =>
                    new
                    {
                        character.Id,
                        character.Name,
                        character.Height,
                        character.Weight,
                        character.HairColor,
                        character.SkinColor,
                        character.EyeColor,
                        character.BirthYear,
                        character.Gender,
                        character.Planet,
                        Movies = character.Movies.Select(movie =>
                            new
                            {
                                movie.Id,
                                movie.Title,
                                movie.Episode,
                            }),
                    })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (planet.Any())
                return Results.Ok(planet);

            return Results.NoContent();
        });

        app.MapGet("/Personagem/{id}", async (
            [FromRoute] int id,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var character = await context
                .Personagens
                .Where(character => character.Id == id)
                .Select(character =>
                    new
                    {
                        character.Id,
                        character.Name,
                        character.Height,
                        character.Weight,
                        character.HairColor,
                        character.SkinColor,
                        character.EyeColor,
                        character.BirthYear,
                        character.Gender,
                        character.Planet,
                        Movies = character.Movies.Select(movie =>
                            new
                            {
                                movie.Id,
                                movie.Title,
                                movie.Episode,
                            }),
                    })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (character is null)
                return Results.NoContent();

            return Results.Ok(character);
        });

        app.MapDelete("/Personagem/{id}", async (
            [FromRoute] int id,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var modelFound = await context
                .Personagens
                .FirstOrDefaultAsync(character => character.Id == id, cancellationToken);

            if (modelFound is null)
                return Results.NotFound();

            context.Personagens.Remove(modelFound);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok(modelFound);
        });

        app.MapPost("/Personagem", async (
            [FromBody] CharacterToAddViewModel modelToAdd,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var modelAlreadyAdded = await context.Personagens.FirstOrDefaultAsync(
                character => character.Name == modelToAdd.Name,
                cancellationToken);

            if (modelAlreadyAdded is not null)
                return Results.Conflict("character name already registered.");

            var planetRelationResult
                = await GetPlanetByIdsAsync(context, modelToAdd.PlanetId, cancellationToken);

            if (planetRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(planetRelationResult.GetErrorMessage("planet"));

            var character = new Personagem
            {
                Name = modelToAdd.Name,
                Height = modelToAdd.Height,
                Weight = modelToAdd.Weight,
                HairColor = modelToAdd.HairColor,
                SkinColor = modelToAdd.SkinColor,
                EyeColor = modelToAdd.EyeColor,
                BirthYear = modelToAdd.BirthYear,
                Gender = modelToAdd.Gender,
                Planet = (Planeta)planetRelationResult.Entities,
            };

            var moviesRelationResult
                = await GetMoviesByIdsAsync(context, modelToAdd.MoviesIds.ToArray(), cancellationToken);

            if (moviesRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(moviesRelationResult.GetErrorMessage("Movies"));

            //character.Planet.AddRange(planetRelationResult.Entities);

            var transaction =
                await context.Database.BeginTransactionAsync(cancellationToken);

            var modelAddedResult = await context.Personagens.AddAsync(character);

            await transaction.CommitAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Created("/Personagem/{id}", modelAddedResult.Entity.Id);
        });

        app.MapPut("/Personagem/{id}", async (
            [FromRoute] int id,
            [FromBody] CharacterToUpdateViewModel modelToUpdate,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            var character = await context.Personagens.FirstOrDefaultAsync(
                character => character.Id == id,
                cancellationToken);

            if (character is null)
                return Results.NotFound("character was not found.");

            var nameAlreadyAdded =
                character.Name != modelToUpdate.Name
                 ? (await context.Personagens.FirstOrDefaultAsync(
                 character => character.Name == modelToUpdate.Name,
                    cancellationToken))
                    is not null
                : false;

            if (nameAlreadyAdded)
                return Results.Conflict("character title already registered.");

            var planetRelationResult
                = await GetPlanetByIdsAsync(context, modelToUpdate.PlanetIdToReplace.ToArray(), cancellationToken);

            if (planetRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(planetRelationResult.GetErrorMessage("planet"));

            var moviesRelationResult
                = await GetMoviesByIdsAsync(context, modelToUpdate.MoviesIdsToReplace.ToArray(), cancellationToken);

            if (moviesRelationResult.ContainsIdsDidntMatch)
                return Results.BadRequest(moviesRelationResult.GetErrorMessage("Movies"));


            var transaction =
                await context.Database.BeginTransactionAsync(cancellationToken);

            character.Name = modelToUpdate.Name;
            character.Height = modelToUpdate.Height;
            character.Weight = modelToUpdate.Weight;
            character.HairColor = modelToUpdate.HairColor;
            character.SkinColor = modelToUpdate.SkinColor;
            character.EyeColor = modelToUpdate.EyeColor;
            character.BirthYear = modelToUpdate.BirthYear;
            character.Gender = modelToUpdate.Gender;
            character.Planet = (Planeta)moviesRelationResult.Entities;

            //
            // Deleting every character and movie to add after
            //
            (await context.Personagens.Include(f => f.Movies).SingleAsync(f => f.Id == character.Id)).Movies.Clear();

            character.Movies.AddRange(moviesRelationResult.Entities);

            await transaction.CommitAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok(id);
        });
    }

    private static async Task<RelationResult<Planeta>> GetPlanetByIdsAsync(
    DataContext context,
    int[] ids,
    CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            return RelationResult<Planeta>.Empty;

        var planet = await context
            .Planetas
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return new(
            IdsDidntMatch: ids.Where(id => !planet.Any(c => c.Id == id)).ToArray(),
            Entities: planet);
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
