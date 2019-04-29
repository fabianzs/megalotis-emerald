using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Entities
{
    public class BadgeLevel
    {
        public long BadgeLevelId { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public Badge Badge { get; set; }
        public ICollection<User> Holders { get; set; }
        public virtual ICollection<UserLevel> UserLevels { get; set; }
       
    }
}