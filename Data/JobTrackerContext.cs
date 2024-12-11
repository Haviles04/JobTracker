using JobTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Data
{
    public class JobTrackerContext : DbContext
    {
        public JobTrackerContext(DbContextOptions<JobTrackerContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .HasOne(j => j.ProjectManager)
                .WithMany() // No navigation in Employee back to Jobs as ProjectManager
                .HasForeignKey(j => j.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Job>()
                .HasMany(j => j.Employees)
                .WithMany(e => e.Jobs)
                .UsingEntity(j => j.ToTable("EmployeeJobs"));


            base.OnModelCreating(modelBuilder);
        }
    }
}
