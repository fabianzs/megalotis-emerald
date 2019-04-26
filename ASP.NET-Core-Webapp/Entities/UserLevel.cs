using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class UserLevel
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long BadgeLevelId { get; set; }
        public BadgeLevel Badgelevel { get; set; }
    }
}
