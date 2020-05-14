using CentralErros.Models;
using CentralErros.Models.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CentralErros.Utils; 
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
                var connection = Utils.Utils.DecryptConnectionString("U2VydmVyPWNlbnRyYWxlcnJvcnMuZGF0YWJhc2Uud2luZG93cy5uZXQ7RGF0YWJhc2U9Q2VudHJhbERlRXJyb3MgO1VzZXI9amFxdWVsYXVyZW50aTsgUGFzc3dvcmQ9QnJAc2lsOTA5MDs=");

                optionsBuilder.UseSqlServer(connection);
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