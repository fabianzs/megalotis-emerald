using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PitchController : ControllerBase
    {
        List<Pitch> pitches = new List<Pitch>();

        [HttpPost("pitches")]
        public IActionResult SavePitches(Pitch newPitch)
        {
            bool badgeNameCheck = false;
            foreach (Pitch actual in pitches) {
                if (actual.BadgeName.Equals(newPitch.BadgeName))
                {
                    badgeNameCheck = true;
                }
                else {
                    badgeNameCheck = false;
                }
            }
            Dictionary<string, object> message = new Dictionary<string,object>();
            if (!badgeNameCheck) {
                pitches.Add(newPitch);
                message.Add("message", "Success");
                return Created("" ,new { message = "Success" });
            }
            else {
                return Created("", new { error = "Unauthorizied" });
            }                
        }
    }
}