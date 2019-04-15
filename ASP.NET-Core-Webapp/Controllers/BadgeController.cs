using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NET_Core_Webapp.Controllers
{
    [Route("badges")]
    public class BadgeController : Controller
    {
        [HttpPost]
        public IActionResult RecieveBadge([FromBody] Badge badge)
        {
            List<string> holdersTest = new List<string>() { "Gazsi", "Géza" };

            if (badge.Levels == null || badge.Name == null || badge.Tag == null || badge.Version == null)
            {

                return NotFound(new { error = "Please provide all fields" });
            }
            else

                badge.Levels.Add(new Skill() { Description = "New test skill added", Level = 500, Holders = holdersTest });
            return Created("/badges", new {message = "Success" });



        }


    }
}
