namespace CodeOrderAPI.ViewModels;

public class MovieToAddViewModel
{
    public string Title { get; set; } = string.Empty;
    public int Episode { get; set; }
    public string OpeningCrawl { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Producer { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public IEnumerable<int> PlanetsIds { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> CharactersIds { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> StarshipsIds { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> VeichlesIds { get; set; } = Enumerable.Empty<int>();
}
