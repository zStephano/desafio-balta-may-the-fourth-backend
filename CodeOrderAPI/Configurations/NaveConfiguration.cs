using CodeOrderAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeOrderAPI.Configurations
{
    public class NaveConfiguration : IEntityTypeConfiguration<Nave>
    {
        public void Configure(EntityTypeBuilder<Nave> builder)
        {
            
        }
    }
}
