using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace THH60Customer.Identity
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
