using System.ComponentModel.DataAnnotations;

namespace JobTracker.Models
{
    public class Employee
    {
        [Key]
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required decimal PayRate { get; set; }
        public ICollection<Job>? Jobs { get; set; }
        public ApplicationUser? User { get; set; }
    }

    public class EmployeeDTO
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public string? Title { get; set; }
        public List<EmployeeJobDTO>? Jobs { get; set; }
    }

    public class EmployeeJobDTO
    {
        public long Id { get; set; }
        public int JobNumber { get; set; }
        public string? Location { get; set; }
    }
}


