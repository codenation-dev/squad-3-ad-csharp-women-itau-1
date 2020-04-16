using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CentralErros.Api.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User  >
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

        }
    }
}
