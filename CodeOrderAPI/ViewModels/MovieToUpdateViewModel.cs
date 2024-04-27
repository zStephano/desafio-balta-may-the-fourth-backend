namespace CodeOrderAPI.ViewModels;

public class MovieToUpdateViewModel
{
    public string Title { get; set; } = string.Empty;
    public int Episode { get; set; }
    public string OpeningCrawl { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Producer { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public IEnumerable<int> PlanetsIdsToReplace { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> CharactersIdsToReplace { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> StarshipsIdsToReplace { get; set; } = Enumerable.Empty<int>();
    public IEnumerable<int> VeichlesIdsToReplace { get; set; } = Enumerable.Empty<int>();
}
