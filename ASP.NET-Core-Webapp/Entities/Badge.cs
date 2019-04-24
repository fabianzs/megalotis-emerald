using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Entities
{
    public class Badge
    {
        public long BadgeId { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public ICollection<BadgeLevel> Levels { get; set; }

        public Badge()
        {
            this.Levels = new List<BadgeLevel>();
        }

        public Badge(string name)
        {
            this.Name = name;
            this.Levels = new List<BadgeLevel>();
        }
    }
}
