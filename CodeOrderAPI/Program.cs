using CodeOrderAPI;
using CodeOrderAPI.Data;
using CodeOrderAPI.Routes;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<DataContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Resolva IMapper aqui
var mapper = app.Services.GetService<IMapper>();

// Passa a instância de IMapper para os métodos que precisam dela
app.MapFilmeEndpoints();
app.MapNaveEndpoints();
app.MapPersonagemEndpoints(mapper);
app.MapPlanetaEndpoints();
app.MapVeiculoEndpoints();

app.Run();
