
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ASP.NET_Core_Webapp.Services;

namespace ASP.NET_Core_Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    public class PitchController : Controller
    {
        ApplicationContext app;
        private readonly SlackService slackService;

        public PitchController(ApplicationContext app, SlackService ss)
        {
            this.app = app;
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
            List<string> reviewers = new List<string>();
            string reviewer1 = "laszlo.molnar25@gmail.com";
            string reviewer2 = "balogh.botond8@gmail.com";
            reviewers.Add(reviewer1);
            reviewers.Add(reviewer2);

            foreach (var reviewer in reviewers)
            {
                slackService.SendEmail(reviewer, "You have been assigned to 1 pitch to make a review.");
            }


            return Created("", new { message = "Success" });
        }
    }
}

