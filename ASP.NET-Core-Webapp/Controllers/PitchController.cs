using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    public class PitchController : Controller
    {
        private readonly ApplicationContext applicationContext;
        private readonly IAuthService authService;

        public PitchController(ApplicationContext application, AuthService authService)
        {
            applicationContext = application;
            this.authService = authService;
        }

        [HttpGet("pitches")]
        public Object GetPitch()
        {
            return StatusCode(401, new { error = "Unauthorizied" });
        }

        [Authorize("Bearer")]
        [HttpPost("pitches")]
        public IActionResult CreateNewPitch(Pitch newPitch)
        {
            var badgeNames = applicationContext.Pitches.Include(a => a.Badge).Select(e => e.Badge.BadgeId);
            long newPitchBadgeId = newPitch.Badge.BadgeId;

            if (!badgeNames.Contains(newPitchBadgeId) && !newPitch.Equals(null)) {
                applicationContext.Add(newPitch);
                applicationContext.SaveChanges();
                return Created("", new { message = "Success" });
            } else {
                return Unauthorized(new { error = "Unauthorizied" });
            }
        }
       
        // notificiton-hoz: elfodagva-> ha a user pitchének a holderében a rewiwerek statusa true;
        // servicebe ksizervezni a logikát
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