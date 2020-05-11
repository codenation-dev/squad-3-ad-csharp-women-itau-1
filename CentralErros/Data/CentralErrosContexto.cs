using CentralErros.Models;
using CentralErros.Models.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CentralErros.Models
{
    public class CentralErroContexto : DbContext
    {
        public DbSet<Environment> Environments { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<ErrorOccurrence> Errors { get; set; }

        public CentralErroContexto(DbContextOptions<CentralErroContexto> options) : base(options)
        {

        }

        public CentralErroContexto()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=centralerrors.database.windows.net;Database=CentralDeErros ;User=jaquelaurenti; " +
                    "Password=Br@sil9090;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EnvironmentConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorConfiguration());
            modelBuilder.ApplyConfiguration(new LevelConfiguration());
        }
    }
}