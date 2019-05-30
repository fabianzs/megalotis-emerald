using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;

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

        [Authorize]
        [HttpGet("pitches")]
        public IActionResult GetPitch()
        {
            string openId = authService.GetOpenIdFromJwtToken(Request);
            var user = database.Users.Include(u => u.Pitches).FirstOrDefault(u => u.OpenId == openId);
            var pitches = user.Pitches.Select(x => new { x.PitchId, x.TimeStamp, x.PitchedLevel, x.PitchMessage }).ToList();
            return Accepted(pitches);
        }

        [Authorize]
        [HttpPost("pitches")]
        public async Task<IActionResult> SendEmailWhenPitchCreated([FromBody] PitchDTO pitchDTO)
        {
            bool postPitch = pitchService.PostPitch(Request, pitchDTO);
            
            if (postPitch)
            {
                foreach (var email in pitchService.CreateEmailListFromPostedPitch(pitchDTO))
                {
                    await slackService.SendEmail(email, $"You have 1 new pitch! Pitch message: {pitchDTO.PitchMessage}");
                }
                return Created("", new { messageSentTo = pitchService.CreateEmailListFromPostedPitch(pitchDTO) });
            }
            return NotFound(new { error = "NotFound" });
        }

        //[Authorize]
        [HttpPut("pitch/{id}")]
        public IActionResult EditPitch([FromRoute] long id, [FromBody] PitchDTO pitchDTO)
        {
            if (pitchDTO == null)
            {
                return StatusCode(404, new { error = "There is no such pitch" });
            }

            if (pitchDTO.BadgeName == null || pitchDTO.PitchMessage == null)
            {
                return NotFound(new { error = "Please provide all fields" });
            }

            string openId = authService.GetOpenIdFromJwtToken(Request);
            pitchService.ModifyPitch(openId, id, pitchDTO);
            return Ok(new { message = "Success" });
        }      
    }
}