
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ASP.NET_Core_Webapp.Controllers
{
    [Route("api")]
    [Authorize("Bearer")]
    [ApiController]
    public class PitchController : Controller
    {
        ApplicationContext ApplicationContext;
        private readonly SlackService slackService;

        public PitchController(ApplicationContext app, SlackService ss)
        {
            this.ApplicationContext = app;
            this.slackService = ss;
        }

        [HttpGet("pitches")]
        public Object GetPitch()
        {
            return StatusCode(401, new { error = "Unauthorizied" });
        }

        [HttpPost("pitches")]
        public IActionResult CreateNewPitch(SeedData.Pitch newPitch)
        {
            List<string> emails = new List<string>();
            foreach (var holder in newPitch.holders)
            {
                var email = ApplicationContext.Users.FirstOrDefault(x => x.Name == holder.name).Email;
                emails.Add(email);
            }

            foreach (var email in emails)
            {
                if (email != null)
                {
                slackService.SendEmail(email, "Testmessage from pitch post....");
                }
            }

            return Created("", new { messageSentTo = emails});
        }
    }
}

