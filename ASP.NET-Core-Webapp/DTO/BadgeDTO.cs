using ASP.NET_Core_Webapp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class BadgeDTO
    {
        public long BadgeId { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public ICollection<BadgeLevelDTO> Levels { get; set; }

        public BadgeDTO()
        {
            Levels = new List<BadgeLevelDTO>();
        }

        public Badge CreateBadge(ApplicationContext applicationContext, BadgeDTO badgeDTO)
        {
            Badge badge = new Badge()
            {
                Name = badgeDTO.Name,
                Version = badgeDTO.Version,
                Tag = badgeDTO.Tag,
            };
            foreach (BadgeLevelDTO bl in badgeDTO.Levels)
            {
                badge.Levels.Add(bl.CreateBadgeLevel(applicationContext));
            }
            return badge;
        }
    }
}
