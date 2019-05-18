using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly ApplicationContext applicationContext;

        public BadgeService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public Dictionary<string, List<MyBadgeDTO>> GetMyBadges(string openId)
        {
            User user = applicationContext.Users
                                            .Include(u => u.UserLevels)
                                            .ThenInclude(ul => ul.BadgeLevel)
                                            .ThenInclude(bl => bl.Badge)
                                            .FirstOrDefault(u => u.OpenId == openId);
            if(user == null)
            {
                throw new UserNotFoundException();
            };

            List<MyBadgeDTO> myBadges = user.UserLevels.Select(ul => new MyBadgeDTO(ul.BadgeLevel.Badge.Name, ul.BadgeLevel.Level)).ToList();
            return new Dictionary<string, List<MyBadgeDTO>>() { { "badges", myBadges } };
        }
    }
}
