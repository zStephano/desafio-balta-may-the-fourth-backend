
namespace CodeOrderAPI.Model
{
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Episodio { get; set; }
        public string TextoAbertura { get; set; }
        public string Diretor { get; set; }
        public string Produtor { get; set; }
        public DateTime DataLancamento { get; set; }
        public Personagem Personagem { get; set; }
        public Planeta Planeta { get; set; }
        public Veiculo Veiculo { get; set; }
        public Nave Nave { get; set; }
        
        // public Filme(int id, string titulo, int episodio, string textoAbertura, string diretor, string produtor, DateTime dataLancamento, Personagem personagem, Planeta planeta, Veiculo veiculo, Nave nave)
        // {
        //     Id = id;
        //     Titulo = titulo;
        //     Episodio = episodio;
        //     TextoAbertura = textoAbertura;
        //     Diretor = diretor;
        //     Produtor = produtor;
        //     DataLancamento = dataLancamento;
        //     Personagem = personagem;
        //     Planeta = planeta;
        //     Veiculo = veiculo;
        //     Nave = nave;
        // }
    }
}
