using CodeOrderAPI.Enum;

namespace CodeOrderAPI.Model
{
    public class Personagem
    {

        public int Id { get; set;}
        public string Name { get; set;} = string.Empty;
        public decimal Height { get; set;}
        public decimal Weight { get; set;}
        public string HairColor { get; set;}= string.Empty;
        public string SkinColor { get; set;}= string.Empty;
        public string EyeColor { get; set;}= string.Empty;
        public  DateTime BirthYear { get; set; }
        public  Genero Gender { get; set; }
        public Planeta? Planet { get; set; } = new Planeta();
        public List<Filme> Movies { get; set; } = new List<Filme>();
    }
}
