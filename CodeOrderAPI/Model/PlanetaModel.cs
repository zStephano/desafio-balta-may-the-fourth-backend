namespace CodeOrderAPI.Model
{
    public class Planeta
    {
        public int Id { get; set;}
        public string Nome { get; set;}
        public string PeriodoRotacao { get; set;}
        public string PeriodoOrbital { get; set;}
        public string Diametro { get; set;}
        public string Clima { get; set;}
        public string Gravidade { get; set;}
        public string Terreno { get; set;}
        public string SuperficeAquatica { get; set;}
        public string Populacao { get; set;}
        public Personagem Personagem { get; set;}
        public Filme Filme { get; set;}
        
    }
}
