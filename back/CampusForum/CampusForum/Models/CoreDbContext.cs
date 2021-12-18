using Microsoft.EntityFrameworkCore;

namespace CampusForum.Models
{
    public class CoreDbContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        
        public CoreDbContext()
        {

        }
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
          

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                optionsBuilder.UseMySQL(@"server = localhost; uid = root; pwd = Huawei@12321;
                    port = 3306; database = dotnet_forum; sslmode = Preferred;");

            }
        }

    }
}