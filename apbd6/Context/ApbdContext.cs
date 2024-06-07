using apbd6.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd6.Context;

public class ApbdContext : DbContext
{
    public ApbdContext()
    {
    }

    public ApbdContext(DbContextOptions<ApbdContext> options) : base(options)
    {
    }
    
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments  { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=db-mssql;Initial Catalog=s24505;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureModels(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureModels(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prescription>().HasKey(p => p.IdPrescription);
        modelBuilder.Entity<Medicament>().HasKey(m => m.IdMedicament);
        modelBuilder.Entity<Prescription_Medicament>().HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);

        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        modelBuilder.Entity<Patient>()
            .Property(p => p.IdPatient)
            .ValueGeneratedNever();
            
        modelBuilder.Entity<Doctor>()
            .Property(d => d.IdDoctor)
            .ValueGeneratedNever();
    }
}