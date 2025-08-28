using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobTracker.Models
{
    public class ApplicationUser: IdentityUser
    {
        public long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }

    public class LoginDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string Title { get; set; }
        public decimal PayRate { get; set; }
    }
}
