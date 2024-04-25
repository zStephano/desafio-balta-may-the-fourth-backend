namespace CodeOrderAPI.ViewModels;

public class MovieToAddViewModel
{
    public string Title { get; set; } = string.Empty;
    public int Episode { get; set; }
    public string OpeningCrawl { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Producer { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public IEnumerable<int> Planets { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> Characters { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> Starships { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> Veichles { get; set; } = Enumerable.Empty<int>();
}
