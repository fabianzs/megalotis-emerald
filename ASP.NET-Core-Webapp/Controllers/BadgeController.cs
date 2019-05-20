using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ASP.NET_Core_Webapp.Services;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_Webapp.Helpers;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class BadgeController : Controller
    {
        private readonly IAuthService authService;
        private readonly IBadgeService badgeService;
        private readonly ApplicationContext applicationContext;

        public BadgeController(IAuthService authService, IBadgeService badgeService, ApplicationContext applicationContext)
        {
            this.authService = authService;
            this.badgeService = badgeService;
            this.applicationContext = applicationContext;
        }

        [Authorize("Bearer")]
        [HttpGet("mybadges")]
        public IActionResult MyBadges()
        {
            string openId = authService.GetOpenIdFromJwtToken(Request);
            return Ok(badgeService.GetMyBadges(openId));
        }

        [HttpPost("badges")]
        public IActionResult RecieveBadge([FromBody]BadgeDTO badgeDTO)
        {
            if (badgeDTO == null)
            {
                return StatusCode(403, new CustomErrorMessage("No message body."));
            }

            if (badgeDTO.Levels == null || !badgeDTO.Levels.Any() || badgeDTO.Name == null || badgeDTO.Tag == null || badgeDTO.Version == null)
            {
                return StatusCode(403, new CustomErrorMessage("Please provide all fields."));
            }

            applicationContext.Badges.Add(badgeDTO.CreateBadge(applicationContext, badgeDTO));
            applicationContext.SaveChanges();
            return Created("/badges", new { message = "Success" });
        }
    }
}
