using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    
    public class PitchController : Controller
    {
        //[AllowAnonymous]
        [HttpGet("pitches")]
        public Object GetPitch()
        {
            List<Holder> Holders = new List<Holder>{
            new Holder("Szabi", "Good", true),
            new Holder("Zsófi", "Good", true),
            new Holder("Laci", "Good", true),
            };

            Pitch pitch = new Pitch("Boti", "C#", 1, 2, "I have been improving", Holders);
            Pitch pitch2 = new Pitch("Boti", "Java", 1, 2, "I stopped learning for a while", Holders);
            Pitch pitch3 = new Pitch("Boti", "Macro", 1, 2, "I have been improving", Holders);
            
            User user = new User(new List<Pitch>{ pitch, pitch2, pitch3 }, new List<Pitch>{ pitch, pitch3 });
            //user = null;
            if(user != null)
            {
                return user;
            }

            return StatusCode(401, new { error = "Unauthorizied" });
        }
    }
}