namespace CodeOrderAPI.ViewModels
{
    public class VehicleViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public decimal CostInCredits { get; set; }
        public decimal Length { get; set; }
        public decimal MaxSpeed { get; set; }
        public int Crew { get; set; }
        public int Passengers { get; set; }
        public decimal CargoCapacity { get; set; }
        public double Consumables { get; set; }
        public string Class { get; set; } = string.Empty;
        public IEnumerable<int> Movies { get; set; } = Enumerable.Empty<int>();

    }
}