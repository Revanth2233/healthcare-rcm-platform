using HealthcareRCM.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthcareRCM.API.Data;

public class RcmDbContext(DbContextOptions<RcmDbContext> options) : DbContext(options)
{
    public DbSet<Claim> Claims => Set<Claim>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Provider> Providers => Set<Provider>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Claim>(e => {
            e.HasKey(x => x.ClaimId);
            e.Property(x => x.BilledAmount).HasColumnType("decimal(10,2)");
            e.Property(x => x.PaidAmount).HasColumnType("decimal(10,2)");
            e.HasIndex(x => x.Status);
            e.HasIndex(x => x.ServiceDate);
            e.HasOne(x => x.Patient).WithMany(p => p.Claims).HasForeignKey(x => x.PatientId);
            e.HasOne(x => x.Provider).WithMany(p => p.Claims).HasForeignKey(x => x.ProviderId);
        });
    }
}
