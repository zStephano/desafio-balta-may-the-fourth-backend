using AutoMapper;
using CodeOrderAPI.Data;
using CodeOrderAPI.Model;
using CodeOrderAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeOrderAPI
{
    public static class PersonagemRoute
    {
        public static void MapPersonagemEndpoints(this WebApplication app, IMapper mapper)
        {
            // Endpoint para adicionar um novo Personagem
            app.MapPost("/Personagem", async (DataContext context, PersonagemToAddViewModel personagemToAdd, CancellationToken cancellationToken) =>
           {
               var personagem = mapper.Map<Personagem>(personagemToAdd);
               context.Personagens.Add(personagem);
               await context.SaveChangesAsync(cancellationToken);

               // Carrega os relacionamentos necessários antes de mapear para DTO
               var carregaPersonagem = await context.Personagens
                   .Include(p => p.Planet.Id)
                   .Include(p => p.Movies)
                   .FirstOrDefaultAsync(p => p.Id == personagem.Id, cancellationToken);

               var resultDto = mapper.Map<PersonagemDTO>(carregaPersonagem);
               return Results.Created($"/Personagem/{personagem.Id}", resultDto);
           });

            // Endpoint para listar todos os Personagens
            app.MapGet("/Personagem", async (DataContext context, CancellationToken cancellationToken) =>
            {
                var personagens = await context.Personagens
                    .Include(p => p.Planet.Id)
                    .Include(p => p.Movies)
                    .ToListAsync(cancellationToken);

                var resultDtos = mapper.Map<List<PersonagemDTO>>(personagens);
                return Results.Ok(resultDtos);
            });


            // Endpoint para buscar um Personagem por ID
            app.MapGet("/Personagem/{id}", async (DataContext context, int id, CancellationToken cancellationToken) =>
              {
                  var personagem = await context.Personagens
                      .Include(p => p.Planet.Id)
                      .Include(p => p.Movies)
                      .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

                  if (personagem == null) return Results.NotFound();

                  var resultDto = mapper.Map<PersonagemDTO>(personagem);
                  return Results.Ok(resultDto);
              });


            // Endpoint para atualizar um Personagem
            app.MapPut("/Personagem/{id}", async (DataContext context, [FromRoute] int id, [FromBody] PersonagemToUpdateViewModel updatedPersonagemViewModel, CancellationToken cancellationToken) =>
            {
                var personagem = await context.Personagens
                    .Include(p => p.Planet.Id) 
                    .Include(p => p.Movies)  
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

                if (personagem == null) return Results.NotFound("Character was not found");

                // Atualiza campos diretos do Personagem
                mapper.Map(updatedPersonagemViewModel, personagem);

                // Atualizar PlanetId, se necessário
                if (personagem.Planet.Id != updatedPersonagemViewModel.PlanetId)
                {
                    personagem.Planet.Id = updatedPersonagemViewModel.PlanetId;
                    // Validação adicional para verificar se o novo PlanetId existe pode ser necessária
                }

                // Atualizar lista de Filme view
                if (updatedPersonagemViewModel.MoviesIdsToReplace != null)
                {
                    var currentMovieIds = personagem.Movies.Select(m => m.Id).ToList();
                    var newMovieIds = updatedPersonagemViewModel.MoviesIdsToReplace.Except(currentMovieIds).ToList();
                    var removedMovieIds = currentMovieIds.Except(updatedPersonagemViewModel.MoviesIdsToReplace).ToList();

                    // Remover filmes não mais associados
                    foreach (var movieId in removedMovieIds)
                    {
                        var filmesParaRemover = personagem.Movies.FirstOrDefault(m => m.Id == movieId);
                        if (filmesParaRemover != null)
                        {
                            personagem.Movies.Remove(filmesParaRemover);
                        }
                    }

                    // Adicionar novos filmes
                    foreach (var movieId in newMovieIds)
                    {
                        var movieToAdd = await context.Filmes.FindAsync(movieId);
                        if (movieToAdd != null)
                        {
                            personagem.Movies.Add(movieToAdd);
                        }
                    }
                }

                await context.SaveChangesAsync(cancellationToken);
                return Results.NoContent();
            });


            // Endpoint para deletar um Personagem
            app.MapDelete("/Personagem/{id}", async (DataContext context, int id, CancellationToken cancellationToken) =>
            {
                var personagem = await context.Personagens
                    .Include(p => p.Movies)  // Inclua relacionamentos que possam precisar de tratamento especial
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

                if (personagem == null) return Results.NotFound("Character not found.");

                // Exemplo de tratamento de relação antes de deletar
                if (personagem.Movies.Any())
                {
                    // Opção 1: Remover relação (desvincular filmes do personagem)
                    personagem.Movies.Clear();
                    // Opção 2: Deletar também os filmes relacionados (se aplicável)
                    context.Filmes.RemoveRange(personagem.Movies);
                }

                context.Personagens.Remove(personagem);
                await context.SaveChangesAsync(cancellationToken);
                return Results.NoContent();
            });

        }
    }
}


