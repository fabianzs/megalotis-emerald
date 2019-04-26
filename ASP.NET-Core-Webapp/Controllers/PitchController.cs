using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    public class PitchController : ControllerBase
    {
        private readonly ApplicationContext applicationContext;

        public PitchController(ApplicationContext application) {
            applicationContext = application;
        }

        [Authorize("Bearer")]
        [HttpPost("pitches")]
        public IActionResult CreateNewPitch(Pitch newPitch)
        {
            var badgeNames = applicationContext.Pitches.Include(a => a.Badge).Select(e => e.Badge.BadgeId);
            long newPitchBadgeId = newPitch.Badge.BadgeId;

            if (!badgeNames.Equals(newPitchBadgeId) && !newPitch.Equals(null)) {
                applicationContext.Add(newPitch);
                applicationContext.SaveChanges();
                return Created("", new { message = "Success" });
            } else {
                return Unauthorized(new { error = "Unauthorizied" });
            }
        }

        [Authorize("Bearer")]
        [HttpPut("pitch/{id}")]
        public IActionResult EditPitch(long id, Pitch pitch) {
            if (id != pitch.PitchId)
            {
                return Unauthorized(new { error = "Unauthorizied" });
            }
            applicationContext.Update(pitch);
            applicationContext.SaveChanges();
            return Ok(new { message = "Success" });
        }
    }
}