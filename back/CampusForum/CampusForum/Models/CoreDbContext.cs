using Microsoft.EntityFrameworkCore;

namespace CampusForum.Models
{
    public class CoreDbContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Album> Album { set; get; }
        public virtual DbSet<Album_picture> Album_picture { set; get; }

        public CoreDbContext()
        {

        }
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }
    }
}