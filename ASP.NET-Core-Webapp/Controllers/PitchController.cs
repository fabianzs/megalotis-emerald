using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class PitchController : Controller
    {

        [HttpGet("pitches")]
        public User GetPitch()
        {
            Holder[] Holders = {
            new Holder("Szabi", "Good", true),
            new Holder("Zsófi", "Good", true),
            new Holder("Laci", "Good", true),
            };

            Pitch pitch = new Pitch("Boti", "C#", 1, 2, "I have been improving", Holders);
            Pitch pitch2 = new Pitch("Boti", "C#", 1, 2, "I have been improving", Holders);
            Pitch pitch3 = new Pitch("Boti", "C#", 1, 2, "I have been improving", Holders);

            User user = new User(new Pitch[] {pitch, pitch2, pitch3}, new Pitch[] { pitch, pitch3 });
            return user;
        }
    }
}