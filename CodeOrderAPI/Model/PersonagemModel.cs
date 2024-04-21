using CodeOrderAPI.Enum;

namespace CodeOrderAPI.Model
{
    public class Personagem
    {

        public int Id { get; set;}
        public string Nome { get; set;} = string.Empty;
        public decimal Height { get; set;}
        public decimal Weight { get; set;}
        public string HairColor { get; set;}= string.Empty;
        public string SkinColor { get; set;}= string.Empty;
        public string EyeColor { get; set;}= string.Empty;
        public  DateTime BirthYear { get; set; }
        public  Genero Gender { get; set; }

        public int PlanetaId { get; set; }
        public Planeta? Planeta { get; set; }
    }
}
