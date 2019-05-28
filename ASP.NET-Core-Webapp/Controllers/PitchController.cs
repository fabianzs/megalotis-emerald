using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.SeedData;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ASP.NET_Core_Webapp.Controllers
{
    public class PitchController : Controller
    {

        ApplicationContext database;
        private readonly IAuthService authService;
        private readonly ISlackService slackService;
        private readonly IPitchService pitchService;

        public PitchController(ApplicationContext db, IAuthService authService, ISlackService ss, IPitchService ps)
        {
            this.database = db;
            this.authService = authService;
            this.slackService = ss;
            this.pitchService = ps;

        }

        [Authorize("Bearer")]
        [HttpGet("pitches")]
        public IActionResult GetPitch()
        {
            string openId = authService.GetOpenIdFromJwtToken(Request);
            var user = database.Users.Include(u => u.Pitches).FirstOrDefault(u => u.OpenId == openId);
            var pitches = user.Pitches.Select(x => new { x.PitchId, x.TimeStamp, x.PitchedLevel, x.PitchMessage }).ToList();
            return Accepted(pitches);
        }

        [Authorize("Bearer")]
        [HttpPost("pitches")]
        public async Task<IActionResult> SendEmailWhenPitchCreated([FromBody]SeedData.Pitch newPitch, [FromBody]PitchDTO pitchDTO)
        {
            bool postPitch = pitchService.PostPitch(Request, pitchDTO);

            if (postPitch)
            {
                foreach (var email in pitchService.CreateEmailListFromPostedPitch(newPitch))
                {
                    await slackService.SendEmail(email, $"You have 1 new pitch! Pitch message: {newPitch.pitchMessage}");
                }
                return Created("", new { messageSentTo = pitchService.CreateEmailListFromPostedPitch(newPitch) });
            }
            return NotFound(new { error = "NotFound" });
        }

        [Authorize("Bearer")]
        [HttpPut("pitch")]
        public IActionResult EditPitch(Entities.Pitch pitch)
        {
            bool putPitch = pitchService.ModifyPitch(pitch, Request);

            if (putPitch)
            {
                return Ok();
            }
            return BadRequest();
        }      
    }
}