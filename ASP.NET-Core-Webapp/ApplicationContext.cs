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
        public DbSet<Holder> Holders { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}