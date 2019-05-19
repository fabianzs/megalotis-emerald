using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.SeedData;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class PitchController : Controller
    {
        ApplicationContext database;
        private ISlackService slackService;
        private readonly IAuthService authService;
        private IPitchService pitchService { get; set; }

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

        [HttpPost("pitches")]
        public async Task<IActionResult> SendEmailWhenPitchCreated([FromBody]SeedData.Pitch newPitch)
        {
            foreach (var email in pitchService.CreateEmailListFromPostedPitch(newPitch))
            {
                await slackService.SendEmail(email, $"You have 1 new pitch! Pitch message: {newPitch.pitchMessage}");
            }
            return Created("", new { messageSentTo = pitchService.CreateEmailListFromPostedPitch(newPitch) });
        }
    }
}