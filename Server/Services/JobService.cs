using JobTracker.Data;
using JobTracker.Models;
using JobTracker.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Services
{
    public class JobService(JobTrackerContext context) : IJobService
    {
        private readonly JobTrackerContext _context = context;

        private bool JobExists(long id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }

        private bool PmExists(long id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public async Task<List<JobDTO>> GetAllJobsAsync(long ProjectManagerId)
        {
            if (!PmExists(ProjectManagerId))
            {
                throw new ArgumentException("Invalid Id or Project Manager doesn't exist");
            }

            var jobs = await _context.Jobs.Where(j => j.ProjectManagerId == ProjectManagerId).ToListAsync();

            return jobs.Select(j => new JobDTO
            {
                Id = j.Id,
                JobNumber = j.JobNumber,
                Location = j.Location
            }).ToList();
        }

        public async Task<JobDTO?> GetJobAsync(long id)
        {
            var job = await _context.Jobs
                        .Include(j => j.ProjectManager)
                        .Include(j => j.Employees)
                        .Include(j => j.Tools)
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
                }).ToList(),
                Tools = job.Tools
            };
        }

        public async Task<JobDTO> CreateJob(JobRequest job)
        {
            var projectManager = await _context.Employees.FindAsync(job.ProjectManagerId);
            if (projectManager == null)
            {
                throw new ArgumentException("Invalid Project Manager ID");
            }

            var employees = job.Employees is not null ? await _context.Employees
                .Where(e => job.Employees.Contains(e.Id))
                .ToListAsync() : null;

            if (job.Employees?.Count != employees?.Count)
            {
                throw new ArgumentException("One of the employee Ids is invalid");
            }

            var tools = job.Tools is not null ? await _context.Tools.Where(t => job.Tools.Contains(t.Id)).ToListAsync() : null;
            if (job.Tools?.Count != tools?.Count)
            {
                throw new ArgumentException("One of the tool Ids is invalid");
            }

            var newJob = new Job
            {
                JobNumber = job.JobNumber,
                Location = job.Location,
                ProjectManagerId = job.ProjectManagerId,
                ProjectManager = projectManager,
                Employees = employees,
                Tools = tools
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

        public Task<List<Job>> GetAllJobsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
