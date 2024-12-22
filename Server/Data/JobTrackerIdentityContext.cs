using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Data
{
    public class JobTrackerIdentityContext: IdentityDbContext<IdentityUser>
    {
        public JobTrackerIdentityContext(DbContextOptions<JobTrackerIdentityContext> options) : base(options)
        { }
    }
}
