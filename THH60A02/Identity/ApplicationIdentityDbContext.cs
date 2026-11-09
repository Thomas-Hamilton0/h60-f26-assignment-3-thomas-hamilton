using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TH_H60_A02.Identity
{
    public class ApplicationIdentityDbContext
    : IdentityDbContext<IdentityUser>
    {
        public ApplicationIdentityDbContext(
            DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
