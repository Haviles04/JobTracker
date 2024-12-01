using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobTracker.Models
{
    public class Employee
    {
        [Key]
        public long Id {  get; set; }
        public required string Name { get; set; }
        public string? Title {  get; set; }
        public required decimal PayRate { get; set; }
        public ICollection<Job>? Jobs { get; set; }
    }

    public class EmployeeDTO
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public string? Title { get; set; }
        public List<JobDTO>? Jobs { get; set; }
    }
}

