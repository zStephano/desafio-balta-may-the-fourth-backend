using CodeOrderAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CodeOrderAPI.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configurationDb;
        public DataContext(
            IConfiguration configurationDb)
            => _configurationDb = configurationDb;

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Personagem> Personagens { get; set; }
        public DbSet<Planeta> Planetas { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Nave> Naves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.FilmeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PersonagemConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PlanetaConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.VeiculoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.NaveConfiguration());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_configurationDb.GetConnectionString("SQLConnection"));

    }
}
