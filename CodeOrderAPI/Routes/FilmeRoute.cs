using CodeOrderAPI.Data;
using CodeOrderAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return Results.BadRequest("");

            var transaction = 
                await context.Database.BeginTransactionAsync(cancellationToken);

            var modelAddedResult = await context.Filmes.AddAsync(new Model.Filme
            {
                Director = modelToAdd.Director,
                Episode = modelToAdd.Episode,
                OpeningCrawl = modelToAdd.OpeningCrawl,
                Producer = modelToAdd.Producer,
                Title = modelToAdd.Title,
                ReleaseDate = modelToAdd.ReleaseDate
            });

            var modelAdded = modelAddedResult.Entity;

            

            await transaction.CommitAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            throw new NotImplementedException();
        });
    }
}
