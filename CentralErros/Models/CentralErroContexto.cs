using CentralErros.Models;
using CentralErros.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CentralErros.Models
{
    public class CentralErroContexto : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Environment> Environments { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<ErrorOccurrence> Errors { get; set; }

        public CentralErroContexto(DbContextOptions<CentralErroContexto> options) : base(options)
        {

        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //confirmação de configuraão para utilizar com In Memory Database
            //if (!optionsBuilder.IsConfigured)
            //optionsBuilder.UseSqlServer("Server=localhost,1433;Database=CentralErros;User Id =sa;Password=jaque@123;Trusted_Connection=False;");

            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseInMemoryDatabase(databaseName: "CentralErros");
            //}

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=centralerrors.database.windows.net;Database=CentralDeErros ;User=jaquelaurenti; " +
                    "Password=Br@sil9090;");
            //optionsBuilder.UseSqlServer("Server=localhost;Database=CentralErros ;User= sa; Password= 123; Trusted_Connection=True");

            //optionsBuilder.UseSqlite("Data Source=nome-do-arq.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<User>().HasMany(u => u.ErrorOccurrences).WithOne(u => u.User).IsRequired();
            modelBuilder.Entity<Level>().HasMany(l => l.ErrorOccurrences).WithOne(l => l.Level).IsRequired();
            modelBuilder.Entity<Environment>().HasMany(e => e.ErrorOccurrences).WithOne(e => e.Environment).IsRequired();
            modelBuilder.Entity<ErrorOccurrence>().HasKey(e => e.Id);

        }
    }
}