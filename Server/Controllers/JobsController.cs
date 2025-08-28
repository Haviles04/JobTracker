using Microsoft.AspNetCore.Mvc;
using JobTracker.Models;
using JobTracker.Services;

namespace JobTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController(JobService jobService) : ControllerBase
    {
        private readonly JobService _jobService = jobService;

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs(long ProjectManagerId)
        {
            var jobs = await _jobService.GetAllJobsAsync(ProjectManagerId);
            return Ok(jobs);
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobDTO>> GetJob(long id)
        {
            var jobDTO = await _jobService.GetJobAsync(id);

            if (jobDTO == null)
            {
                return NotFound($"Job with Id {id} not found.");
            }

            return Ok(jobDTO);
        }

        // PUT: api/Jobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(long id, JobRequest job)
        {
            try
            {
                await _jobService.UpdateJob(id, job);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error {e.Message}");
            }
        }

        // POST: api/Jobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Job>> CreateJob(JobRequest job)
        {
            try
            {
                var newJob = await _jobService.CreateJob(job);
                return CreatedAtAction("GetJob", new { id = newJob.Id }, newJob);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error{e.Message}");
            }
        }

        //// DELETE: api/Jobs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteJob(long id)
        //{
        //    var job = await _context.Jobs.FindAsync(id);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Jobs.Remove(job);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


    }
}
