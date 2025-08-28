using JobTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Data
{
    public class JobTrackerContext : IdentityDbContext<IdentityUser>
    {
        public JobTrackerContext(DbContextOptions<JobTrackerContext> options) : base(options) { }

        public required DbSet<Employee> Employees { get; set; }
        public required DbSet<Job> Jobs { get; set; }
        public required DbSet<Tool> Tools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Job>()
                .HasOne(j => j.ProjectManager)
                .WithMany()
                .HasForeignKey(j => j.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Job>()
                .HasMany(j => j.Employees)
                .WithMany(e => e.Jobs)
                .UsingEntity(j => j.ToTable("EmployeeJobs"));

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
