using CodeOrderAPI.Enum;

public class PersonagemDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string HairColor { get; set; }
    public string SkinColor { get; set; }
    public string EyeColor { get; set; }
    public DateTime BirthYear { get; set; }
    public Genero Gender { get; set; }
    public int PlanetId { get; set; }
    public List<int> MoviesIds { get; set; }
}