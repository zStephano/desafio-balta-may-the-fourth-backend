using CodeOrderAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeOrderAPI.Configurations
{
    public class PersonagemConfiguration :  IEntityTypeConfiguration<Personagem>
    {

        public void Configure(EntityTypeBuilder<Personagem> builder)
        {
    
        }
    }
}
