using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ASP.NET_Core_Webapp.Services;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class BadgeController : Controller
    {
        private readonly IAuthService authService;
        private readonly ApplicationContext applicationContext;

        public BadgeController(IAuthService authService, ApplicationContext applicationContext)
        {
            this.authService = authService;
            this.applicationContext = applicationContext;
        }

        [Authorize("Bearer")]
        [HttpGet("mybadges")]
        public IActionResult MyBadges()
        {
            string openId = authService.GetOpenIdFromJwtToken(Request);
            //User _user = applicationContext.Users.FirstOrDefault(u => u.OpenId == openId);
            //InitializeDb(_user);
            User user = applicationContext.Users.Include(u => u.UserLevels).ThenInclude(ul => ul.Badgelevel).ThenInclude(bl => bl.Badge).Where(u => u.OpenId == openId).FirstOrDefault();
            return Ok(new Dictionary<string, object>() { { "badges", user.UserLevels.Select(ul => new { ul.Badgelevel.Badge.Name, ul.Badgelevel.Level }) } });
        }

        [HttpPost("badges")]
        public IActionResult RecieveBadge([FromBody]BadgeDTO badgeDTO)
        {
            if (badgeDTO == null)
            {
                return StatusCode(404, new { error = "No message body" });
            }

            if (badgeDTO.Levels == null || badgeDTO.Name == null || badgeDTO.Tag == null || badgeDTO.Version == null)
            {
                return NotFound(new { error = "Please provide all fields" });
            }

            Badge badge = new Badge()
            {
                Name = badgeDTO.Name,
                Version = badgeDTO.Version,
                Tag = badgeDTO.Tag,
            };

            foreach (BadgeLevelDTO bl in badgeDTO.Levels)
            {
                BadgeLevel newBadgeLevel = new BadgeLevel() { Level = bl.Level, Description = bl.Description, UserLevels = new List<UserLevel>() };
                foreach (string holder in bl.Holders)
                {
                    UserLevel newUserLevel = new UserLevel() { Badgelevel = newBadgeLevel, User = applicationContext.Users.Include(u => u.UserLevels).ThenInclude(ul => ul.Badgelevel).ThenInclude(blvl => blvl.Badge).FirstOrDefault(u => u.Name.Equals(holder)) };
                    newBadgeLevel.UserLevels.Add(newUserLevel);
                }
                badge.Levels.Add(newBadgeLevel);
            }
            applicationContext.Badges.Add(badge);
            applicationContext.SaveChanges();
            return Created("/badges", new { message = "Success" });
        }

        [Authorize("Bearer")]
        [HttpGet("mybadgesmock")]
        public IActionResult MyBadgesMock()
        {
            List<Badge> badgesList = new List<Badge>();

            badgesList.Add(new Badge("test"));
            return Ok(new { badges = badgesList });
        }

        [Authorize("Bearer")]
        [HttpPost("badgesmock")]
        public IActionResult RecieveBadgeMock([FromBody]Badge badge)
        {
            if (badge == null)
            {
                return StatusCode(404, new { error = "No message body" });
            }

            if (badge.Levels == null || badge.Name == null || badge.Tag == null || badge.Version == null)
            {
                return NotFound(new { error = "Please provide all fields" });
            }
            return Created("/badges", new { message = "Success" });
        }

        public void InitializeDb(User user)
        {
            UserLevel userlevel1 = new UserLevel() { Badgelevel = new BadgeLevel() { Level = 2, Badge = new Badge("English speaker") }, User = user };
            UserLevel userlevel2 = new UserLevel() { Badgelevel = new BadgeLevel() { Level = 3, Badge = new Badge("Java developer") }, User = user };
            UserLevel userlevel3 = new UserLevel() { Badgelevel = new BadgeLevel() { Level = 1, Badge = new Badge("Stress management") }, User = user };
            user.UserLevels = new List<UserLevel>();
            user.UserLevels.Add(userlevel1);
            user.UserLevels.Add(userlevel2);
            user.UserLevels.Add(userlevel3);
            applicationContext.Users.Update(user);
            applicationContext.SaveChanges();
        }
    }
}
