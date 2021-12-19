using Microsoft.EntityFrameworkCore;

namespace CampusForum.Models
{
    public class CoreDbContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Follow> Follow { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<StateText> State_text { get; set; }
        public virtual DbSet<Like> like_state { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        
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