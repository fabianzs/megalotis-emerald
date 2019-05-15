
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Newtonsoft.Json.Linq;

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
            Pitch pitchToAdd = new Pitch() { };
            // Add user:
            User userToAdd = new User();
            userToAdd = ApplicationContext.Users.Where(x => x.Name == newPitch.username).FirstOrDefault();
            // Add reviews:
            foreach (var holderReview in newPitch.holders)
            {
                Review review = new Review(holderReview.message, holderReview.pitchStatus);
                pitchToAdd.Holders.Add(review);
                ApplicationContext.SaveChanges();

                review.User = ApplicationContext.Users.Where(x => x.Name == holderReview.name).FirstOrDefault();
                review.Pitch = ApplicationContext.Pitches.Where(x => x.PitchMessage == holderReview.message).FirstOrDefault();

                ApplicationContext.Reviews.Add(review);
                ApplicationContext.SaveChanges();
            }

            if (userToAdd == null)
            {
                User newUser = new User() { Name = newPitch.username };
                //ApplicationContext.Add(newUser);
                pitchToAdd.User = newUser;
                ApplicationContext.SaveChanges();
            }
            else
            {
                pitchToAdd.User = userToAdd;
                ApplicationContext.SaveChanges();
            }

            Badge badgeToAdd;
            badgeToAdd = ApplicationContext.Badges.Where(x => x.Name == newPitch.badgeName).FirstOrDefault();
            pitchToAdd.Badge = badgeToAdd;

            pitchToAdd.TimeStamp = DateTime.Now;
            pitchToAdd.PitchMessage = newPitch.pitchMessage;
            pitchToAdd.Badge = ApplicationContext.Badges.Where(x => x.Name == newPitch.badgeName).FirstOrDefault();
            pitchToAdd.BadgeLevel = ApplicationContext.BadgeLevels.Where(x => x.Badge.Name == newPitch.badgeName).FirstOrDefault();
            ApplicationContext.Add(pitchToAdd);

            ApplicationContext.SaveChanges();

            List<string> emails = new List<string>();
            foreach (var holder in newPitch.holders)
            {
                var email = ApplicationContext.Users.FirstOrDefault(x => x.Name == holder.name).Email;
                if (email != null)
                {
                    emails.Add(email);
                }
            }

            foreach (var email in emails)
            {
                slackService.SendEmail(email, $"You have 1 new pitch! Pitch message: {newPitch.pitchMessage}");
            }
            return Created("", new {messageSentTo=emails, pitches = ApplicationContext.Pitches.Select(x=>x.PitchMessage).ToList(), pitchToAddTimeStamp = pitchToAdd.TimeStamp });
        }
    }
}

