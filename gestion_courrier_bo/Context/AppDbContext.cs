using gestion_courrier_bo.Models;
using Microsoft.EntityFrameworkCore;

namespace gestion_courrier_bo.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Poste> Postes { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Flag> Flags { get; set; }
        public DbSet<StatusCourrier> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employe>()
                .HasOne(e => e.Poste);

            modelBuilder.Entity<Employe>()
                .HasOne(e => e.Departement);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=gestionCourrier;User ID=ZOTOAVINA\\ASUS;Password=;trusted_connection=true;");
        }
    }
}
