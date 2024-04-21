using CodeOrderAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeOrderAPI.Configurations
{
    public class PlanetaConfiguration : IEntityTypeConfiguration<Planeta>
    {
        public void Configure(EntityTypeBuilder<Planeta> builder)
        {
            builder.ToTable("Planeta");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(500);
            builder.Property(x => x.RotationPeriod).HasMaxLength(500);
            builder.Property(x => x.OrbitalPeriod).HasMaxLength(500);
            builder.Property(x => x.Diameter).HasMaxLength(500);
            builder.Property(x => x.Climate).HasMaxLength(500);
            builder.Property(x => x.Gravity).HasMaxLength(500);
            builder.Property(x => x.Terrain).HasMaxLength(500);
            builder.Property(x => x.SurfaceWater).HasMaxLength(500);
            builder.Property(x => x.Population).HasMaxLength(500);

            builder.HasMany(x => x.Characters).WithOne(c => c.Planet).HasForeignKey("CharacterId");
            builder.HasMany(x => x.Movies).WithMany(c => c.Planets).UsingEntity("FilmesPlanets");
        }
    }
}
