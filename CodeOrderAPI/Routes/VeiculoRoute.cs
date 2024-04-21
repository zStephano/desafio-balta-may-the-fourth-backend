using CodeOrderAPI.Model;

namespace CodeOrderAPI
{
    public static class VeiculoRoute
    {
        public static void MapVeiculoEndpoints(this WebApplication app)
        {
            var summaries = new[]
            {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

            app.MapGet("/Veiculo", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
                return forecast;
            })
            .WithName("Veiculo")
            .WithOpenApi();
        }
       
    }
}
