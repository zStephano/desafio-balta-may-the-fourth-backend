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
         {
            {
                "name": "Galactic Explorer 55",
                "model": "GX-3",
                "manufacturer": "Interstellar Shipworks",
                "costInCredits": 800000,
                "length": 85,
                "maxSpeed": 880,
                "crew": 10,
                "passengers": 40,
                "cargoCapacity": 200000,
                "hyperdriveRating": 1.5,
                "mglt": 70,
                "consumables": 85,
                "class": "Explorer",
              "moviesIds": [
                1
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


            foreach (var movieId in modelToAdd.MoviesIds.ToList())
            {
                var movieDb = await context.Filmes.FirstOrDefaultAsync(m => m.Id == movieId);
                if (movieDb != null)
                {
                    newStarship.Movies.Add(movieDb);
                }                
            }
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


}
