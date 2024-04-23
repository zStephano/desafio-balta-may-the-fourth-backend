namespace CodeOrderAPI.Model
{
    public class Planeta
    {
        public int Id { get; set;}
        public string Name { get; set;} = string.Empty;
        public TimeSpan RotationPeriod { get; set;}
        public TimeSpan OrbitalPeriod { get; set;}
        public decimal Diameter { get; set; }
        public string Climate { get; set; } = string.Empty;
        public decimal Gravity { get; set; }
        public string Terrain { get; set; } = string.Empty;
        public int SurfaceWater { get; set; }
        public long Population { get; set; }
        public List<Personagem> Characters { get; set; } = new List<Personagem>();
        public List<Filme> Movies { get; set; } = new List<Filme>();
    }
}
