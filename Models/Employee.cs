using System.ComponentModel.DataAnnotations;

namespace JobTracker.Models
{
    public class Employee
    {
        [Key]
        public long Id {  get; set; }
        public required string Name { get; set; }
        public string? Title {  get; set; }
        public required decimal PayRate { get; set; }

    }
}
