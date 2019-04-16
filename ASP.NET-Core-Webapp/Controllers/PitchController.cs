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
    [Route("api")]
    [ApiController]
    public class PitchController : ControllerBase
    {
        List<Pitch> pitches = new List<Pitch>();

        [HttpPost("pitches")]
        public IActionResult CreateNewPitch(Pitch newPitch)
        {        
            if (!pitches.Exists(e => e.BadgeName.Equals(newPitch.BadgeName))) {
                pitches.Add(newPitch);               
                return Created("",new { message = "Success" });
            }else {
                return Unauthorized( new { error = "Unauthorizied" });
            }                     
        }
    }
}