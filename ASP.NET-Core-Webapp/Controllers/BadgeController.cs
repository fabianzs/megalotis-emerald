
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
            User user = applicationContext.Users.Include(u => u.UserLevels).ThenInclude(ul => ul.Badgelevel).ThenInclude(bl => bl.Badge).Where(u => u.OpenId == openId).FirstOrDefault();
            return Ok(new Dictionary<string, object>() { { "badges", user.UserLevels.Select(ul => new { ul.Badgelevel.Badge.Name, ul.Badgelevel.Level }) } });
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
    }
}
