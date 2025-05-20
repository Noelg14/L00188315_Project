using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace L00188315_Project.Infrastructure.Data.Identity
{
    /// <summary>
    /// AppIdentityDbContext is a custom DbContext class that inherits from IdentityDbContext.
    /// </summary>
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Constructor for AppIdentityDbContext
        /// </summary>
        /// <param name="options"></param>
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }

        /// <summary>
        /// Override of the OnModelCreating method
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
