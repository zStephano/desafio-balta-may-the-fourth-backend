
namespace CodeOrderAPI.Model
{
    public class Filme
    {
        public int Id { get; set; }
        public string Title { get; set; }= string.Empty;
        public int Episode { get; set; }
        public string OpeningCrawl { get; set; }= string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Producer { get; set; }= string.Empty;
        public DateTime ReleaseDate { get; set; }

        public List<Planeta> Planetas { get; set; } = new List<Planeta>();
        public List<Personagem> Personagens { get; set; } = new List<Personagem>();
        public List<Nave> Starships { get; set; } = new List<Nave>();
        public List<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
    }
}
