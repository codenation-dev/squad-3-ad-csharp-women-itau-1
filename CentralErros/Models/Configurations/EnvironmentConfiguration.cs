using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CentralErros.Models.Configurations
{
    public class EnvironmentConfiguration : IEntityTypeConfiguration<Environment>
    {
        public void Configure(EntityTypeBuilder<Environment> builder)
        {
            builder.HasKey(x => x.Id);

        }
    }
}
