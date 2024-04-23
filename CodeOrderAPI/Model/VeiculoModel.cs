namespace CodeOrderAPI.Model
{
    public class Veiculo
    {
        public int Id { get; set;}
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public decimal CostInCredits { get; set; }
        public decimal Length { get; set; }
        public decimal MaxSpeed { get; set; }
        public int Crew { get; set; }
        public int Passengers { get; set; }
        public decimal CargoCapacity { get; set; }
        public TimeSpan Consumables { get; set; }
        public string Class { get; set; } = string.Empty;

        public List<Filme> Movies {get; set;} = new();
    }
}
