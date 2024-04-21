using CodeOrderAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeOrderAPI.Configurations
{
    public class PlanetaConfiguration : IEntityTypeConfiguration<Planeta>
    {
        public void Configure(EntityTypeBuilder<Planeta> builder)
        {
            
        }
    }
}
