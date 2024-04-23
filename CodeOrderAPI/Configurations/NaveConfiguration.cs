using CodeOrderAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeOrderAPI.Configurations
{
    public class NaveConfiguration : IEntityTypeConfiguration<Nave>
    {
        public void Configure(EntityTypeBuilder<Nave> builder)
        {
            builder.ToTable("Nave");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(500);
            builder.Property(x => x.Model).HasMaxLength(500);
            builder.Property(x => x.Manufacturer).HasMaxLength(500);
            builder.Property(x => x.CostInCredits);
            builder.Property(x => x.Length);
            builder.Property(x => x.MaxSpeed);
            builder.Property(x => x.Crew);
            builder.Property(x => x.Passengers);
            builder.Property(x => x.CargoCapacity);
            builder.Property(x => x.HyperdriveRating);
            builder.Property(x => x.Mglt);
            builder.Property(x => x.Consumables).HasMaxLength(500);
            builder.Property(x => x.Class).HasMaxLength(500);

            builder.HasMany(x => x.Movies).WithMany(c => c.Starships).UsingEntity("FilmesNaves");

        }
    }
}
