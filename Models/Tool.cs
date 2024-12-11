using System.ComponentModel.DataAnnotations;

namespace JobTracker.Models
{
    public class Tool
    {
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }
    }
}
