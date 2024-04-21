using CodeOrderAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeOrderAPI.Configurations
{
    public class PersonagemConfiguration :  IEntityTypeConfiguration<Personagem>
    {

        public void Configure(EntityTypeBuilder<Personagem> builder)
        {
            builder.ToTable("Personagem");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(500);
            builder.Property(x => x.Height);
            builder.Property(x => x.Weight);
            builder.Property(x => x.HairColor).HasMaxLength(500);
            builder.Property(x => x.SkinColor).HasMaxLength(500);
            builder.Property(x => x.EyeColor).HasMaxLength(500);
            builder.Property(x => x.BirthYear);
            builder.Property(x => x.Gender);

            builder.HasOne(x => x.Planet).WithMany(c => c.Characters).HasForeignKey("PlanetId");
            builder.HasMany(x => x.Movies).WithMany(c => c.Characters).UsingEntity(t => t.ToTable("FilmesPersonagens"));
        }
    }
}
