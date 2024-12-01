using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobTracker.Models
{
    public class Job
    {
        [Key]
        public long Id { get; set; }
        public int JobNumber { get; set; }
        public string? Location { get; set; }
        [ForeignKey("ProjectManager")]
        public long? ProjectManagerId { get; set; }
        public Employee? ProjectManager { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }

    public class JobRequest
    {
        public long Id { get; set; }
        public int JobNumber { get; set; }
        public long ProjectManagerId { get; set; }
        public string Location { get; set; }
        public List<long> Employees { get; set; }
    }

    public class JobDTO
    {
        public long Id { get; set; }
        public int JobNumber { get; set; }
        public string? Location { get; set; }
        public EmployeeDTO? ProjectManager { get; set; }
        public List<EmployeeDTO>? Employees { get; set; }
    }
}
