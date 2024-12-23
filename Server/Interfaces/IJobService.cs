using JobTracker.Models;

namespace JobTracker.Interfaces
{
    public interface IJobService
    {
        Task<List<Job>> GetAllJobsAsync();
        Task<JobDTO?> GetJobAsync(long id);
        Task<JobDTO> CreateJob(JobRequest job);
        Task UpdateJob(long id, JobRequest job);
    }
}
