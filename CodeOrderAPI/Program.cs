using CodeOrderAPI;
using CodeOrderAPI.Data;
using CodeOrderAPI.Routes;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<DataContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StarWars", Version = "v1" });
    c.UseInlineDefinitionsForEnums();
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
    options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapFilmeEndpoints();
app.MapNaveEndpoints();
app.MapPersonagemEndpoints();
app.MapPlanetaEndpoints();
app.MapVeiculoEndpoints();


app.Run();
