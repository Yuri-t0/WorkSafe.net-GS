using Microsoft.EntityFrameworkCore;
using WorkSafe.Api.Domain.Entities;

namespace WorkSafe.Api.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Workstation> Workstations => Set<Workstation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var workstation = modelBuilder.Entity<Workstation>();

            workstation.ToTable("Workstations");

            workstation.HasKey(w => w.Id);

            workstation.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            workstation.Property(w => w.EmployeeName)
                .IsRequired()
                .HasMaxLength(100);

            workstation.Property(w => w.Department)
                .IsRequired()
                .HasMaxLength(100);

            workstation.Property(w => w.MonitorDistanceCm)
                .IsRequired();

            workstation.Property(w => w.HasAdjustableChair)
                .IsRequired();

            workstation.Property(w => w.HasFootrest)
                .IsRequired();

            workstation.Property(w => w.ErgonomicRiskLevel)
                .HasConversion<int>()
                .IsRequired();

            workstation.Property(w => w.LastEvaluationDate)
                .IsRequired();

         }
    }
}