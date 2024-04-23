using CodeOrderAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeOrderAPI.Configurations
{
    public class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("Veiculo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Model).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Manufacturer).IsRequired().HasMaxLength(500);
            builder.Property(x => x.CostInCredits);
            builder.Property(x => x.Length);
            builder.Property(x => x.MaxSpeed);
            builder.Property(x => x.Crew);
            builder.Property(x => x.Passengers);
            builder.Property(x => x.CargoCapacity);
            builder.Property(x => x.Consumables);
            builder.Property(x => x.Class).IsRequired().HasMaxLength(500);

            builder.HasMany(x => x.Movies).WithMany(c => c.Veichles).UsingEntity("FilmesVeiculos");
        }
    }
}
