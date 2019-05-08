
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
            

            //List<string> reviewers = new List<string>();
            //string reviewer1 = "laszlo.molnar25@gmail.com";
            //string reviewer2 = "balogh.botond8@gmail.com";
            //reviewers.Add(reviewer1);
            //reviewers.Add(reviewer2);

            foreach (var email in emails)
            {
                slackService.SendEmail(email, "Testmessage from pitch post....");
            }

            return Created("", new { message = "Success" });
        }
    }
}

