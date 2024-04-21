using CodeOrderAPI.Enum;

namespace CodeOrderAPI.Model
{
    public class Personagem
    {

        public int Id { get; set;}
        public string Nome { get; set;}
        public Decimal Altura { get; set;}
        public Decimal Peso { get; set;}
        public string CorCabelo { get; set;}
        public string CorPele { get; set;}
        public string CorOlhos { get; set;}
        public  string Nascimento { get; set;}
        public  Genero Genero { get; set;}
        public Planeta Planeta { get; set;}
        public Filme Filme{ get; set;}

    }
}
