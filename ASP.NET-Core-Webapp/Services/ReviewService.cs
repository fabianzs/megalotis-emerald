using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationContext applicationContext;
        private readonly ISlackService slackService;

        public ReviewService(ApplicationContext applicationContext, ISlackService slackService)
        {
            this.applicationContext = applicationContext;
            this.slackService = slackService;
        }

        public async Task CreateReview(string openId, ReviewDTO reviewDTO)
        {
            Review newReview = new Review()
            {
                Message = reviewDTO.Message,
                Status = reviewDTO.Status,
                Pitch = applicationContext.Pitches
                                            .Include(p => p.User)
                                            .Include(p => p.BadgeLevel)
                                            .ThenInclude(bl => bl.Badge)
                                            .FirstOrDefault(p => p.PitchId == reviewDTO.PitchId),
                User = applicationContext.Users
                                            .Include(u => u.UserLevels)
                                            .ThenInclude(ul => ul.BadgeLevel)
                                            .ThenInclude(bl => bl.Badge)
                                            .FirstOrDefault(u => u.OpenId == openId)
            };

            if (newReview.Pitch == null)
            {
                throw new PitchIsNullException();
            }

            CheckIfReviewerHasPitchedBadge(newReview);

            applicationContext.Add(newReview);
            applicationContext.SaveChanges();

            string slackMessage = $"You have received a new {(newReview.Status == true ? "positive" : "negative")} review with the following message: \"{newReview.Message}\"";
            await slackService.SendEmail(newReview.Pitch.User.Email, slackMessage);
        }

        public void CheckIfReviewerHasPitchedBadge(Review newReview)
        {
            BadgeLevel badgeLevelOfPitch = newReview.Pitch.BadgeLevel;
            BadgeLevel badgeLevelOfReviewer = newReview.User.UserLevels
                                                                .Select(ul => ul.BadgeLevel)
                                                                .FirstOrDefault(bl => bl.Badge.Name.ToLower().Contains(badgeLevelOfPitch.Badge.Name.ToLower()));

            if (badgeLevelOfPitch == null || badgeLevelOfReviewer == null || badgeLevelOfPitch.Badge.Name != badgeLevelOfReviewer.Badge.Name || newReview.User == newReview.Pitch.User)
            {
                throw new NotAllowedToReviewException();
            }
        }

        public async Task UpdateReview(string openId, ReviewDTO reviewDTO, long id)
        {
            Review reviewToUpdate = applicationContext.Reviews 
                                                        .Include(r => r.User)
                                                        .Include(r => r.Pitch)
                                                        .ThenInclude(p => p.User)
                                                        .FirstOrDefault(r => r.ReviewId == id);

            string slackMessage = $"Your {(reviewToUpdate.Status == true ? "positive" : "negative")} review with the following message: \"{reviewToUpdate.Message}\" has been changed. The updated review is {(reviewDTO.Status == true ? "positive" : "negative")} and has the following message: \"{reviewDTO.Message}\"";

            if (reviewToUpdate == null)
            {
                throw new ReviewIsNullException();
            }

            reviewToUpdate.Message = reviewDTO.Message;
            reviewToUpdate.Status = reviewDTO.Status;

            if (reviewToUpdate.Pitch.PitchId != reviewDTO.PitchId)
            {
                throw new InvalidPitchException();
            }

            if (reviewToUpdate.User.OpenId != openId)
            {
                throw new OtherUsersReviewException();
            }

            applicationContext.Reviews.Update(reviewToUpdate);
            applicationContext.SaveChanges();

            await slackService.SendEmail(reviewToUpdate.Pitch.User.Email, slackMessage);
        }
    }
}
