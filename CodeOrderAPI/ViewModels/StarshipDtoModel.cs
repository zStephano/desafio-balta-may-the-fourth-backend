namespace CodeOrderAPI.ViewModels
{
    public class StarshipDtoModel
    {
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string CostInCredits { get; set; } = string.Empty;
        public string Length { get; set; } = string.Empty;
        public string MaxSpeed { get; set; } = string.Empty;
        public int Crew { get; set; }
        public int Passengers { get; set; }
        public string CargoCapacity { get; set; } = string.Empty;
        public decimal HyperdriveRating { get; set; }
        public int Mglt { get; set; }
        public string Consumables { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public List<MovieDto> Movies { get; set; } = new();
    }

    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

}
