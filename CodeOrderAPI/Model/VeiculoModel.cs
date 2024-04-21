namespace CodeOrderAPI.Model
{
    public class Veiculo
    {

        public int Id { get; set;}
        public string Nome { get; set; }
        public string Modelo { get; set; }
        public string Fabricante { get; set; }
        public int Custo { get; set; }
        public int Comprimento { get; set; }
        public int VelocidadeMaxima { get; set; }
        public int Tripulacao { get; set; }
        public int Passageiros { get; set; }
        public string CapacidadeCarga { get; set; }
        public string Consumiveis { get; set; }
        public string Classe { get; set; }
        public Filme Filme { get; set; }

    }
}
