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
        private SlackService slackService;
        private readonly IAuthService authService;

        public PitchController(ApplicationContext db, IAuthService authService, SlackService ss)
        {
            this.database = db;
            this.authService = authService;
            this.slackService = ss;
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
        public async Task<IActionResult> CreateNewPitch([FromBody]SeedData.Pitch newPitch)
        {
            List<Holder> holderList = new List<Holder>();
            List<string> emailList = new List<string>();
            string emailToAdd = String.Empty;
            foreach (var holder in newPitch.holders)
            {
                emailToAdd = database.Users.FirstOrDefault(x => x.Name == holder.name).Email;
                if (emailToAdd != null)
                {
                    emailList.Add(emailToAdd);
                }
            }

            foreach (var email in emailList)
            {
                await slackService.SendEmail(email, $"You have 1 new pitch! Pitch message: {newPitch.pitchMessage}");
            }
            return Created("", new { messageSentTo = emailList });
        }
    }
}