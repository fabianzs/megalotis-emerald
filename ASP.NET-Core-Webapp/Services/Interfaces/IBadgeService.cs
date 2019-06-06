using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public interface IBadgeService
    {
        Dictionary<string, List<MyBadgesDTO>> GetMyBadges(string openId);
        void CheckBadgeDTO(BadgeDTO badgeDTO);
        void CreateBadge(BadgeDTO badgeDTO);
        BadgeLevel CreateBadgeLevel(BadgeLevelDTO badgeLevelDTO);
        UserLevel CreateUserLevel(BadgeLevel newBadgeLevel, string holder);        
    }
}
