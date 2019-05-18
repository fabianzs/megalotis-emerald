using ASP.NET_Core_Webapp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class BadgeLevelDTO
    {
        public long BadgeLevelId { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public Badge Badge { get; set; }
        public virtual ICollection<String> Holders { get; set; }

        public BadgeLevelDTO()
        {
            Holders = new List<String>();
        }

        public BadgeLevel CreateBadgeLevel(ApplicationContext applicationContext)
        {
            BadgeLevel newBadgeLevel = new BadgeLevel() { Level = this.Level, Description = this.Description, UserLevels = new List<UserLevel>() };
            foreach (string holder in this.Holders)
            {
                newBadgeLevel.UserLevels.Add(this.CreateUserLevel(applicationContext, newBadgeLevel, holder));
            }
            return newBadgeLevel;
        }

        public UserLevel CreateUserLevel(ApplicationContext applicationContext, BadgeLevel newBadgeLevel, string holder)
        {
            return new UserLevel() { BadgeLevel = newBadgeLevel, User = applicationContext.Users.Include(u => u.UserLevels).ThenInclude(ul => ul.BadgeLevel).ThenInclude(blvl => blvl.Badge).FirstOrDefault(u => u.Name.Equals(holder)) };
        }
    }
}
