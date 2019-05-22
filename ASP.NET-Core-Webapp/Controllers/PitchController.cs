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

            User user = applicationContext.Users.Include(a => a.Pitches).ThenInclude(p=>p.Badge).FirstOrDefault(u => u.OpenId == openId);
            List<string> badgeNames = user.Pitches.Select(p => p.Badge.Name).ToList();

            if (!badgeNames.Contains(newPitch.Badge.Name) && !newPitch.Equals(null))
            {
                newPitch.User = user;
                applicationContext.Add(newPitch);
                applicationContext.SaveChanges();
                return Created("", new { message = "Success" });
            }
            return NotFound(new { error = "NotFound" });
        }
       
        [Authorize("Bearer")]
        [HttpPut("pitch")]
        public IActionResult EditPitch(Pitch pitch) {

            string openId = authService.GetOpenIdFromJwtToken(Request);
        
            User user = applicationContext.Users.Include(a => a.Pitches).FirstOrDefault(u => u.OpenId == openId);
            List<long> userPitchId = user.Pitches.Select(p => p.PitchId).ToList();
            pitch.User = user;

            if (applicationContext.Pitches.Select(e => e.PitchId).Contains(pitch.PitchId) && userPitchId.Contains(pitch.PitchId) ) {
                applicationContext.Update(pitch);
                applicationContext.SaveChanges();
                return Ok();
            }       
            return BadRequest();
        }
    }
}

