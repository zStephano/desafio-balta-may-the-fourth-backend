namespace CodeOrderAPI.ViewModels;

public class PlanetToUpdateViewModel
{
    public string Name { get; set; } = string.Empty;
    public TimeSpan RotationPeriod { get; set; }
    public TimeSpan OrbitalPeriod { get; set; }
    public decimal Diameter { get; set; }
    public string Climate { get; set; } = string.Empty;
    public decimal Gravity { get; set; }
    public string Terrain { get; set; } = string.Empty;
    public int SurfaceWater { get; set; }
    public long Population { get; set; }
    public IEnumerable<int> CharactersIdsToReplace { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> MoviesIdsToReplace { get; set; } = Enumerable.Empty<int>();
}
