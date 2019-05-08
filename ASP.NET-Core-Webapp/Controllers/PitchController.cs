
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
        public IActionResult CreateNewPitch(Pitch newPitch)
        {

            Pitch pitchToAdd = new Pitch() { Badge = newPitch.Badge, BadgeLevel = newPitch.BadgeLevel, Holders = newPitch.Holders, PitchedLevel = newPitch.PitchedLevel, PitchMessage = newPitch.PitchMessage, TimeStamp = DateTime.Now, User = newPitch.User };
            
            ApplicationContext.Add(pitchToAdd);
            ApplicationContext.SaveChanges();

            List<User> users = new List<User>();

            var holders2 = pitchToAdd.Holders.Select(x => new User() {Email = x.User.Email, Name = x.User.Name }).ToList();
            //List<string> reviewers = new List<string>();
            //string reviewer1 = "laszlo.molnar25@gmail.com";
            //string reviewer2 = "balogh.botond8@gmail.com";
            //reviewers.Add(reviewer1);
            //reviewers.Add(reviewer2);

            foreach (var user in holders2)
            {
                slackService.SendEmail(user.Email, "Testmessage from pitch post....");
            }

            return Created("", new { message = "Success" });
        }
    }
}

