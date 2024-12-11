using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JobTracker.Models
{
    public class Tool
    {
        public long Id { get; set; }
        [StringLength(50)]
        public required string Name { get; set; }
        [DefaultValue(false)]
        public required bool NeedsRepair {get; set;}
    }
}
