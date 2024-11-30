using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ICollection<Employee>? Employees { get; set;}
    }
}
