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
            var naves = await context.Naves
                .Include(n => n.Movies)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!naves.Any())
            {
                return Results.NoContent();
            }

            var navesDto = naves.Select(nave => new StarshipDtoModel
            {
                Name = nave.Name,
                Model = nave.Model,
                Manufacturer = nave.Manufacturer,
                CostInCredits = nave.CostInCredits.ToString("N0"), // Formato como número com separadores
                Length = $"{nave.Length} meters",
                MaxSpeed = $"{nave.MaxSpeed} km/h",
                Crew = nave.Crew,
                Passengers = nave.Passengers,
                CargoCapacity = $"{nave.CargoCapacity} kg",
                HyperdriveRating = nave.HyperdriveRating,
                Mglt = nave.Mglt,
                Consumables = $"{(nave.Consumables.TotalDays / 30).ToString("0.##")} month",
                Class = nave.Class,
                Movies = nave.Movies.Select(m => new MovieDto { Id = m.Id, Title = m.Title }).ToList()
            }).ToList();

            return Results.Ok(navesDto);
        }).WithTags("Nave");

        app.MapGet("/nave/{id}", async ([FromRoute] int id, DataContext context, CancellationToken cancellationToken) =>
        {
            var nave = await context.Naves
                .Include(n => n.Movies)
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

            if (nave == null)
            {
                return Results.NotFound($"Nave with ID {id} not found.");
            }

            // Mapeando a entidade para o DTO
            var naveDto = new StarshipDtoModel
            {
                Name = nave.Name,
                Model = nave.Model,
                Manufacturer = nave.Manufacturer,
                CostInCredits = nave.CostInCredits.ToString("N0"), // Formato como número com separadores
                Length = $"{nave.Length} meters",
                MaxSpeed = $"{nave.MaxSpeed} km/h",
                Crew = nave.Crew,
                Passengers = nave.Passengers,
                CargoCapacity = $"{nave.CargoCapacity} kg",
                HyperdriveRating = nave.HyperdriveRating,
                Mglt = nave.Mglt,
                Consumables = $"{(nave.Consumables.TotalDays / 30).ToString("0.##")} month",
                Class = nave.Class,
                Movies = nave.Movies.Select(m => new MovieDto { Id = m.Id, Title = m.Title }).ToList()
            };

            return Results.Ok(naveDto);
        }).WithTags("Nave");

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
                Consumables = TimeSpan.FromDays(modelToAdd.Consumables * 30), // grava em meses
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
        }).WithTags("Nave");

        app.MapPut("/nave/{id}", async ([FromRoute] int id, [FromBody] StarshipToUpdateViewModel modelToUpdate, DataContext context, CancellationToken cancellationToken) =>
        {
            var existingNave = await context.Naves.Include(n => n.Movies).FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
            if (existingNave == null)
            {
                return Results.NotFound($"Nave with ID {id} not found.");
            }

            // Atualizando propriedades da nave
            existingNave.Name = modelToUpdate.Name;
            existingNave.Model = modelToUpdate.Model;
            existingNave.Manufacturer = modelToUpdate.Manufacturer;
            existingNave.CostInCredits = modelToUpdate.CostInCredits;
            existingNave.Length = modelToUpdate.Length;
            existingNave.MaxSpeed = modelToUpdate.MaxSpeed;
            existingNave.Crew = modelToUpdate.Crew;
            existingNave.Passengers = modelToUpdate.Passengers;
            existingNave.CargoCapacity = modelToUpdate.CargoCapacity;
            existingNave.HyperdriveRating = modelToUpdate.HyperdriveRating;
            existingNave.Mglt = modelToUpdate.Mglt;
            existingNave.Consumables = TimeSpan.FromDays(modelToUpdate.Consumables);
            existingNave.Class = modelToUpdate.Class;

            // Atualizando os filmes associados à nave
            var moviesRelationResult = await GetMoviesByIdsAsync(context, modelToUpdate.MoviesIds.ToArray(), cancellationToken);
            if (moviesRelationResult.ContainsIdsDidntMatch)
            {
                return Results.BadRequest(moviesRelationResult.GetErrorMessage("Movie"));
            }

            // Limpar lista existente e adicionar novos
            existingNave.Movies.Clear();
            existingNave.Movies.AddRange(moviesRelationResult.Entities);

            // Salvando alterações
            context.Naves.Update(existingNave);
            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok($"Nave with ID {id} updated.");
        }).WithTags("Nave");

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
        }).WithTags("Nave");
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
