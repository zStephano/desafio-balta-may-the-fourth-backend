using CodeOrderAPI.Data;
using CodeOrderAPI.Model;
using CodeOrderAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CodeOrderAPI
{
    public static class VeiculoRoute
    {
        public static void MapVeiculoEndpoints(this WebApplication app)
        {
            app.MapPost("/veiculo", async ([FromBody] VehicleViewModel vehicleToAdd, DataContext ctx, CancellationToken cancellationToken) =>
            {
                var newVehicle = new Veiculo
                {
                    Name = vehicleToAdd.Name,
                    Model = vehicleToAdd.Model,
                    Manufacturer = vehicleToAdd.Manufacturer,
                    CostInCredits = vehicleToAdd.CostInCredits,
                    Length = vehicleToAdd.Length,
                    MaxSpeed = vehicleToAdd.MaxSpeed,
                    Crew = vehicleToAdd.Crew,
                    Passengers = vehicleToAdd.Passengers,
                    CargoCapacity = vehicleToAdd.CargoCapacity,
                    Consumables = TimeSpan.FromDays(vehicleToAdd.Consumables * 365),
                    Class = vehicleToAdd.Class
                };

                var moviesRelationResult = await GetMoviesByIdsAsync(ctx, vehicleToAdd.Movies.ToArray(), cancellationToken);

                if (moviesRelationResult.ContainsIdsDidntMatch)
                    return Results.BadRequest(moviesRelationResult.GetErrorMessage("Movie"));

                newVehicle.Movies.AddRange(moviesRelationResult.Entities);

                ctx.Veiculos.Add(newVehicle);
                await ctx.SaveChangesAsync(cancellationToken);

                return Results.Created($"/veiculo/{newVehicle.Id}", newVehicle);
            });

            app.MapGet("/veiculo", async (DataContext ctx, CancellationToken cancellationToken) =>
            {
                var vehicles = await ctx.Veiculos.Include(n => n.Movies).AsNoTracking().ToListAsync(cancellationToken);

                if (!vehicles.Any())
                    return Results.NoContent();

                var vehiclesDto = vehicles.Select(vehicle => new
                {
                    name = vehicle.Name,
                    model = vehicle.Model,
                    manufacturer = vehicle.Manufacturer,
                    costInCredits = vehicle.CostInCredits.ToString(),
                    length = $"{vehicle.Length} meters",
                    maxSpeed = $"{vehicle.MaxSpeed} km/h",
                    crew = $"{vehicle.Crew}",
                    passengers = $"{vehicle.Passengers}",
                    cargoCapacity = $"{vehicle.CargoCapacity} kg",
                    consumables = $"{(vehicle.Consumables.TotalDays / 365).ToString("N0")} years",
                    Class = vehicle.Class,
                    movies = vehicle.Movies.Select(m => new { id = m.Id, title = m.Title }).ToList()

                }).ToList();

                return Results.Ok(vehiclesDto);
            });

            app.MapGet("/veiculo/{id}", async ([FromRoute] int id, DataContext ctx, CancellationToken cancellationToken) =>
            {
                var vehicle = await ctx.Veiculos.Include(n => n.Movies).FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

                if (vehicle == null)
                    return Results.NotFound($"Vehicle with ID {id} not found.");

                var vehiclesDto = new
                {
                    name = vehicle.Name,
                    model = vehicle.Model,
                    manufacturer = vehicle.Manufacturer,
                    costInCredits = vehicle.CostInCredits.ToString(),
                    length = $"{vehicle.Length} meters",
                    maxSpeed = $"{vehicle.MaxSpeed} km/h",
                    crew = $"{vehicle.Crew}",
                    passengers = $"{vehicle.Passengers}",
                    cargoCapacity = $"{vehicle.CargoCapacity} kg",
                    consumables = $"{(vehicle.Consumables.TotalDays / 365).ToString("N0")} years",
                    Class = vehicle.Class,
                    movies = vehicle.Movies.Select(m => new { id = m.Id, title = m.Title }).ToList()

                };

                return Results.Ok(vehiclesDto);
            });

            app.MapPut("/veiculo/{id}", async ([FromRoute] int id, [FromBody] VehicleViewModel vehicleToUpdate, DataContext ctx, CancellationToken cancellationToken) =>
            {
                var vehicle = await ctx.Veiculos.Include(n => n.Movies).FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

                if (vehicle == null)
                    return Results.NotFound($"Vehicle with ID {id} not found.");

                vehicle.Name = vehicleToUpdate.Name;
                vehicle.Model = vehicleToUpdate.Model;
                vehicle.Manufacturer = vehicleToUpdate.Manufacturer;
                vehicle.CostInCredits = vehicleToUpdate.CostInCredits;
                vehicle.Length = vehicleToUpdate.Length;
                vehicle.MaxSpeed = vehicleToUpdate.MaxSpeed;
                vehicle.Crew = vehicleToUpdate.Crew;
                vehicle.Passengers = vehicleToUpdate.Passengers;
                vehicle.CargoCapacity = vehicleToUpdate.CargoCapacity;
                vehicle.Consumables = TimeSpan.FromDays(vehicleToUpdate.Consumables);
                vehicle.Class = vehicleToUpdate.Class;

                var moviesRelationResult = await GetMoviesByIdsAsync(ctx, vehicleToUpdate.Movies.ToArray(), cancellationToken);

                if (moviesRelationResult.ContainsIdsDidntMatch)
                    return Results.BadRequest(moviesRelationResult.GetErrorMessage("Movie"));

                vehicle.Movies.Clear();
                vehicle.Movies.AddRange(moviesRelationResult.Entities);

                ctx.Veiculos.Update(vehicle);
                await ctx.SaveChangesAsync(cancellationToken);

                return Results.Ok($"Vehicle with ID {id} updated.");
            });

            app.MapDelete("/veiculo/{id}", async ([FromRoute] int id, DataContext ctx, CancellationToken cancellationToken) =>
            {
                var vehicle = await ctx.Veiculos.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

                if (vehicle == null)
                    return Results.NotFound($"Vehicle with ID {id} not found.");

                ctx.Veiculos.Remove(vehicle);
                await ctx.SaveChangesAsync(cancellationToken);
                return Results.Ok($"Vehicle with ID {id} deleted.");
            });
        }
        private static async Task<RelationResult<Filme>> GetMoviesByIdsAsync(DataContext ctx, int[] ids, CancellationToken cancellationToken = default)
        {
            if (!ids.Any())
                return RelationResult<Filme>.Empty;

            var filmes = await ctx
                .Filmes
                .Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellationToken);

            return new(
                IdsDidntMatch: ids.Where(id => !filmes.Any(v => v.Id == id)).ToArray(),
                Entities: filmes);
        }
        private record RelationResult<TEntity>(int[] IdsDidntMatch, IEnumerable<TEntity> Entities)
        {
            public static readonly RelationResult<TEntity> Empty = new(Array.Empty<int>(), Enumerable.Empty<TEntity>());

            public bool ContainsIdsDidntMatch => IdsDidntMatch.Length > 0;

            public string GetErrorMessage(string fieldName)
            {
                return $"{fieldName} ids '{string.Join(", ", IdsDidntMatch)}' did not match.";
            }
        }
    }
}
