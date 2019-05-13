using ASP.NET_Core_Webapp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Webapp.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<Pitch> Pitches { get; set; }
        public DbSet<BadgeLevel> BadgeLevels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLevel>()
               .HasKey(sc => new { sc.BadgeLevelId, sc.UserId });
            modelBuilder.Entity<Pitch>().Property(s => s.TimeStamp).HasColumnName("Created");
            modelBuilder.Entity<Badge>().Property(s => s.TimeStamp).HasColumnName("Created");
        }
    }
}
