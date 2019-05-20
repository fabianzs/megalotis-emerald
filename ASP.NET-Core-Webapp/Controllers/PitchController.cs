using System;
using System.Collections.Generic;
using System.Linq;
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ASP.NET_Core_Webapp.Controllers
{
    public class PitchController : Controller
    {
        private readonly ApplicationContext applicationContext;
        private readonly IAuthService authService;

        public PitchController(ApplicationContext application, IAuthService authService)
        {
            this.authService = authService;
            this.applicationContext = application;
        }


        [Authorize("Bearer")]
        [HttpGet("pitches")]
        public IActionResult GetPitch()
        {
            string openId = authService.GetOpenIdFromJwtToken(Request);
            var user = applicationContext.Users.Include(u => u.Pitches).FirstOrDefault(u => u.OpenId == openId);
            var pitches = user.Pitches.Select(x => new { x.PitchId, x.TimeStamp, x.PitchedLevel, x.PitchMessage }).ToList();
            return Accepted(pitches);
        }

        [Authorize("Bearer")]
        [HttpPost("pitches")]
        public IActionResult CreateNewPitch(Pitch newPitch)
        {
          
            string openId = authService.GetOpenIdFromJwtToken(Request);

            User user = applicationContext.Users.Include(a => a.Pitches).FirstOrDefault(u => u.OpenId == openId);
            List<long> userPitchId = user.Pitches.Select(p => p.PitchId).ToList();

            if (!applicationContext.Pitches.Select(e => e.PitchId).Contains(newPitch.PitchId) && !userPitchId.Contains(newPitch.PitchId) && !newPitch.Equals(null))
            {
                applicationContext.Add(newPitch);
                applicationContext.SaveChanges();
                return Created("", new { message = "Success" });
            }
            return Unauthorized(new { error = "Unauthorizied" });
        }
       
        [Authorize("Bearer")]
        [HttpPut("pitch")]
        public IActionResult EditPitch(Pitch pitch) {

            string openId = authService.GetOpenIdFromJwtToken(Request);
        
            User user = applicationContext.Users.Include(a => a.Pitches).FirstOrDefault(u => u.OpenId == openId);
            List<long> userPitchId = user.Pitches.Select(p => p.PitchId).ToList();
           
            if (applicationContext.Pitches.Select(e => e.PitchId).Contains(pitch.PitchId) && userPitchId.Contains(pitch.PitchId) ) {
                applicationContext.Update(pitch);
                applicationContext.SaveChanges();
                return Ok();
            }       
            return BadRequest();
        }
    }
}

