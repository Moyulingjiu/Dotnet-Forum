using Microsoft.EntityFrameworkCore;

namespace CampusForum.Models
{
    public class CoreDbContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }
    }
}