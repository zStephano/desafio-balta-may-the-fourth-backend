using CodeOrderAPI.Data;
using CodeOrderAPI.Model;
using CodeOrderAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CodeOrderAPI.Routes;

public static class NaveRoute
{
    public static void MapNaveEndpoints(this WebApplication app)
    {

        app.MapGet("/nave", async (DataContext context, CancellationToken cancellationToken) =>
        {
            var naves = await context.Naves.AsNoTracking().ToListAsync(cancellationToken);
            return naves.Any() ? Results.Ok(naves) : Results.NoContent();
        });

        app.MapGet("/nave/{id}", async ([FromRoute] int id, DataContext context, CancellationToken cancellationToken) =>
        {
            var nave = await context.Naves.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
            return nave != null ? Results.Ok(nave) : Results.NotFound($"Nave with ID {id} not found.");
        });


        /*
         * JSON para TESTE de POST
             {
              "name": "Galactic Enterprise 1705",
              "model": "GX-5",
              "manufacturer": "string",
              "costInCredits": 500200,
              "length": 93,
              "maxSpeed": 200500,
              "crew": 30,
              "passengers": 1200,
              "cargoCapacity": 2000000,
              "hyperdriveRating": 9,
              "mglt": 50,
              "consumables": 10,
              "class": "string",
              "moviesIds": [
                1,3
              ]
            }
                     */


        app.MapPost("/nave", async (
            [FromBody] StarshipToAddViewModel modelToAdd,
            DataContext context,
            CancellationToken cancellationToken) =>
        {
            // Não pode ter naves com o mesmo nome (???)
            bool modelAlreadyAdded = await context.Naves
                .AnyAsync(n => n.Name == modelToAdd.Name, cancellationToken);

            if (modelAlreadyAdded)
                return Results.Conflict("Starship title already registered.");

            var newStarship = new Nave
            {
                Name = modelToAdd.Name,
                Model = modelToAdd.Model,
                Manufacturer = modelToAdd.Manufacturer,
                CostInCredits = modelToAdd.CostInCredits,
                Length = modelToAdd.Length,
                MaxSpeed = modelToAdd.MaxSpeed,
                Crew = modelToAdd.Crew,
                Passengers = modelToAdd.Passengers,
                CargoCapacity = modelToAdd.CargoCapacity,
                HyperdriveRating = modelToAdd.HyperdriveRating,
                Mglt = modelToAdd.Mglt,
                Consumables = TimeSpan.FromDays(modelToAdd.Consumables),
                Class = modelToAdd.Class
            };

            var moviesRelationResult = await GetMoviesByIdsAsync(context, modelToAdd.MoviesIds.ToArray(), cancellationToken);
            if (moviesRelationResult.ContainsIdsDidntMatch)
            {
                return Results.BadRequest(moviesRelationResult.GetErrorMessage("Movie"));
            }
            newStarship.Movies.AddRange(moviesRelationResult.Entities);

            context.Naves.Add(newStarship);
            await context.SaveChangesAsync(cancellationToken);

            return Results.Created($"/nave/{newStarship.Id}", newStarship);
        });


        app.MapDelete("/nave/{id}", async ([FromRoute] int id, DataContext context, CancellationToken cancellationToken) =>
        {
            var nave = await context.Naves.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
            if (nave == null)
            {
                return Results.NotFound($"Nave with ID {id} not found.");
            }

            context.Naves.Remove(nave);
            await context.SaveChangesAsync(cancellationToken);
            return Results.Ok($"Nave with ID {id} deleted.");
        });
    }

    private static async Task<RelationResult<Filme>> GetMoviesByIdsAsync(
        DataContext context,
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            return RelationResult<Filme>.Empty;

        var filmes = await context
            .Filmes
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return new(
            IdsDidntMatch: ids.Where(id => !filmes.Any(v => v.Id == id)).ToArray(),
            Entities: filmes);
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
