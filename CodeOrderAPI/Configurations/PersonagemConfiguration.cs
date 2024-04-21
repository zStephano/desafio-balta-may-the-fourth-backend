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

            builder.Property(x => x.HairColor).HasMaxLength(500);
            builder.Property(x => x.SkinColor).HasMaxLength(500);
            builder.Property(x => x.EyeColor).HasMaxLength(500);

            builder.HasOne(x => x.Planeta).WithMany().HasForeignKey(x => x.PlanetaId);
        }
    }
}
