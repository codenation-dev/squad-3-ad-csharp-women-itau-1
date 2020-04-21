using CentralErros.Api.Models.Configurations;
using CentralErros.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CentralErros.Api.Models
{
    public class CentralErroContexto : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Environment> Environments { get; set; }
        public DbSet<Environment> Levels { get; set; }

        public CentralErroContexto(DbContextOptions<CentralErroContexto> options) : base(options)
        {

        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //confirmação de configuraão para utilizar com In Memory Database
            //if (!optionsBuilder.IsConfigured)
            //optionsBuilder.UseSqlServer("Server=localhost,1433;Database=CentralErros;User Id =sa;Password=jaque@123;Trusted_Connection=False;");

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "CentralErros");
            }

            //optionsBuilder.UseSqlite("Data Source=nome-do-arq.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EnvironmentConfiguration());
            modelBuilder.ApplyConfiguration(new LevelConfiguration());
        }
    }
}