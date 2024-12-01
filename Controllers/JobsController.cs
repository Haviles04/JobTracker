using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobTracker.Models;

namespace JobTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobTrackerContext _context;

        public JobsController(JobTrackerContext context)
        {
            _context = context;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            return await _context.Jobs.Include(j => j.ProjectManager).ToListAsync();
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobDTO>> GetJob(long id)
        {
            var job = await _context.Jobs
                .Include(j => j.ProjectManager)
                .Include(j => j.Employees)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            JobDTO jobDTO = new()
            {
                Id = job.Id,
                JobNumber = job.JobNumber,
                Location = job.Location,
                ProjectManager = new EmployeeDTO
                {
                    Id = job.ProjectManager.Id,
                    Name = job.ProjectManager.Name,
                },
                Employees = job.Employees.Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToList()
            };


            return jobDTO;
        }

        // PUT: api/Jobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(long id, Job job)
        {
            if (id != job.Id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Job>> CreateJob(JobRequest job)
        {
            try
            {
                var projectManager = await _context.Employees.FindAsync(job.ProjectManagerId);
                if (projectManager == null)
                {
                    return BadRequest($"Project Manager with Id {job.ProjectManagerId} does not exist.");
                }

                var employees = await _context.Employees
                  .Where(e => job.Employees.Contains(e.Id))
                  .ToListAsync();

                if (job.Employees.Count != employees.Count)
                {
                    return BadRequest("One of the employee Ids is invalid");
                }

                var newJob = new Job
                {
                    JobNumber = job.JobNumber,
                    Location = job.Location,
                    ProjectManagerId = job.ProjectManagerId,
                    ProjectManager = projectManager,
                    Employees = employees
                };

                _context.Jobs.Add(newJob);
                await _context.SaveChangesAsync();

                JobDTO jobDTO = new()
                {
                    Id = newJob.Id,
                    JobNumber = newJob.JobNumber,
                    Location = newJob.Location,
                    ProjectManager = new EmployeeDTO
                    {
                        Id = newJob.ProjectManager.Id,
                        Name = newJob.ProjectManager.Name
                    },
                    Employees = newJob.Employees.Select(e => new EmployeeDTO
                    {
                        Id = e.Id,
                        Name = e.Name
                    }).ToList()
                };


                return CreatedAtAction("GetJob", new { id = newJob.Id }, jobDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(long id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobExists(long id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
