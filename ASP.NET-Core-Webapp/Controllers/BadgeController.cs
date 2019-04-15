using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ASP.NET_Core_Webapp.Entities;

using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class BadgeController : Controller
    {
        [HttpGet("mybadges")]
        public IActionResult MyBadges()
        {
            User user = new User();

            List<Badge> badges = new List<Badge>();

            badges.Add(new Badge("test", 4));
            badges.Add(new Badge("test", 4));
            badges.Add(new Badge("test", 4));

            return Ok(new {badges = badges });
            
        }

        
    }
}