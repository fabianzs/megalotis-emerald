<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
=======
﻿using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
>>>>>>> origin/Mock_Post_Badges_Endpoint

namespace ASP.NET_Core_Webapp.Controllers
{
    public class BadgeController : Controller
    {
<<<<<<< HEAD
        [HttpGet("mybadges")]
        public IActionResult MyBadges()
        {
            List<Badge> badges = new List<Badge>();

            badges.Add(new Badge("test", 4));
            return Ok(new { badges = badges });

        }
        
    }
}
=======
       
        [HttpPost("badges")]
        public IActionResult RecieveBadge([FromBody]Badge badge, [FromQuery]int authorized)
        {
            List<string> holdersTest = new List<string>() { "Gazsi", "Géza" };
            if (badge == null)
            {
                return StatusCode(404, new { error = "No message body" });
            }

            //if (authorized.Equals(1))
            //{
                if (badge.Levels == null || badge.Name == null || badge.Tag == null || badge.Version == null)
                {
                    return NotFound(new { error = "Please provide all fields" });
                }

                badge.Levels.Add(new LevelEntity() { Description = "New test skill added", Level = 500, Holders = holdersTest });

                return Created("/badges", new { message = "Success" });

            //}
            //else

            //    return StatusCode(401, new { error = "Unauthorized" });
        }
    }
}
>>>>>>> origin/Mock_Post_Badges_Endpoint
