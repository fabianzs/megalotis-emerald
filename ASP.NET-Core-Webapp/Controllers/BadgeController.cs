using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class BadgeController : Controller
    {

        [HttpGet("mybadges")]
        public IActionResult MyBadges()
        {
            List<Badge> badgesList = new List<Badge>();

            badgesList.Add(new Badge("test"));
            return Ok(new { badges = badgesList });
        }

        [HttpPost("badges")]
        public IActionResult RecieveBadge([FromBody]Badge badge)
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
