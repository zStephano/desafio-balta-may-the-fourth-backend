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
            builder.Property(x => x.Nome).HasMaxLength(500);
            builder.Property(x => x.RotationPeriod).HasMaxLength(500);
            builder.Property(x => x.OrbitalPeriod).HasMaxLength(500);
            builder.Property(x => x.Terrain).HasMaxLength(500);

            builder.HasMany(x => x.Characters).WithMany(c => c.Planet);
        }
    }
}
