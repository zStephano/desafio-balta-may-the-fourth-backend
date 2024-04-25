using CodeOrderAPI.Data;
using CodeOrderAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace CodeOrderAPI.Routes
{
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

        }
    }
}
