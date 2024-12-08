using JobTracker.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Services
{
    public class JobsService(JobTrackerContext context)
    {
        private readonly JobTrackerContext _context = context;

        private bool JobExists(long id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }

        public async Task<List<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs
                .Include(j => j.ProjectManager)
                .ToListAsync();
        }

        public async Task<JobDTO?> GetJobAsync(long id)
        {
            var job = await _context.Jobs
                        .Include(j => j.ProjectManager)
                        .Include(j => j.Employees)
                        .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                throw new ArgumentException("Job Not Found");
            }

            return new JobDTO
            {
                Id = job.Id,
                JobNumber = job.JobNumber,
                Location = job.Location,
                ProjectManager = job.ProjectManager != null
                    ? new JobEmployeeDTO
                    {
                        Id = job.ProjectManager.Id,
                        Name = job.ProjectManager.Name,
                        Title = job.ProjectManager.Title,
                    }
                    : null,
                Employees = job.Employees?.Select(e => new JobEmployeeDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Title = e.Title
                }).ToList()
            };
        }

        public async Task<JobDTO> CreateJob(JobRequest job)
        {
            var projectManager = await _context.Employees.FindAsync(job.ProjectManagerId);
            if (projectManager == null)
            {
                throw new ArgumentException("Invalid Project Manager ID");
            }

            var employees = await _context.Employees
                .Where(e => job.Employees.Contains(e.Id))
                .ToListAsync();

            if (job.Employees.Count != employees.Count)
            {
                throw new ArgumentException("One of the employee Ids is invalid");
            }

            var newJob = new Job
            {
                JobNumber = job.JobNumber,
                Location = job.Location,
                ProjectManagerId = job.ProjectManagerId,
                ProjectManager = projectManager,
                Employees = employees,
                Tools = job.Tools
            };

            _context.Jobs.Add(newJob);
            await _context.SaveChangesAsync();

            JobDTO jobDTO = new()
            {
                Id = newJob.Id,
                JobNumber = newJob.JobNumber,
                Location = newJob.Location,
                ProjectManager = newJob.ProjectManager != null ? new JobEmployeeDTO
                {
                    Id = newJob.ProjectManager.Id,
                    Name = newJob.ProjectManager.Name
                } : null,
                Employees = newJob.Employees?.Select(e => new JobEmployeeDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Title = e.Title
                }).ToList()
            };
            return jobDTO;
        }

        public async Task UpdateJob(long id, JobRequest job)
        {
            if (id != job.Id)
            {
                throw new ArgumentException("Job Id doesn't match");
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    throw new ArgumentException("Invalid Job Id");
                }
                else
                {
                    throw;
                }
            }

            return;
        }

    }
}
