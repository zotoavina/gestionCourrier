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
        public DbSet<Courrier> Courriers { get; set; }
        public DbSet<CourrierDestinataire> CourrierDestinataires { get; set; }
        public DbSet<HistoriqueCourrierDestinataire> Historiques { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employe>()
                .HasOne(e => e.Poste);

            modelBuilder.Entity<Employe>()
                .HasOne(e => e.Departement);

            modelBuilder.Entity<CourrierDestinataire>()
                .HasKey(cd => new { cd.IdCourrier, cd.IdDestinataire });

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Courrier)
                .WithMany(c => c.Destinataires)
                .HasForeignKey(cd => cd.IdCourrier)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Destinataire)
                .WithMany(d => d.CourrierDestinataires )
                .HasForeignKey(cd => cd.IdDestinataire)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Recepteur)
                .WithMany()
                .HasForeignKey(cd => cd.IdRecepteur)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Coursier)
                .WithMany()
                .HasForeignKey(cd => cd.IdCoursier)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourrierDestinataire>()
                .HasOne(cd => cd.Status)
                .WithMany()
                .HasForeignKey(cd => cd.IdStatus)
                .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=gestionCourrier;User ID=ZOTOAVINA\\ASUS;Password=;trusted_connection=true;");
        }
    }
}
