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
    [Authorize]
    public class BadgeController : Controller
    {
        private readonly IAuthService authService;
        private readonly IBadgeService badgeService;

        public BadgeController(IAuthService authService, IBadgeService badgeService)
        {
            this.authService = authService;
            this.badgeService = badgeService;
        }

        [HttpGet("mybadges")]
        public IActionResult MyBadges()
        {
            string openId = authService.GetOpenIdFromJwtToken(Request);
            return Ok(badgeService.GetMyBadges(openId));
        }

        [HttpPost("badges")]
        public IActionResult RecieveBadge([FromBody] BadgeDTO badgeDTO)
        {
            badgeService.CreateBadge(badgeDTO);
            return Created("/badges", new { message = "Success" });
        }
    }
}
