using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ASP.NET_Core_Webapp.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationContext applicationContext;

        public ReviewService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public void CreateReview(string openId, ReviewDTO reviewDTO)
        {
            Review newReview = new Review()
            {
                Message = reviewDTO.Message,
                Status = reviewDTO.Status,
                Pitch = applicationContext.Pitches.Include(p => p.BadgeLevel).ThenInclude(bl => bl.Badge).FirstOrDefault(p => p.PitchId == reviewDTO.PitchId),
                User = applicationContext.Users.Include(u => u.UserLevels).ThenInclude(ul => ul.Badgelevel).ThenInclude(bl => bl.Badge).Where(u => u.OpenId == openId).FirstOrDefault()
            };
            
            if(newReview.Pitch == null)
            {
                throw new Exception("The provided pitch does not exist.");
            }

            BadgeLevel badgeLevelOfPitch = newReview.Pitch.BadgeLevel;
            BadgeLevel badgeLevelOfReviewer = newReview.User.UserLevels.Select(ul => ul.Badgelevel).FirstOrDefault(bl => bl.Badge.Name == badgeLevelOfPitch.Badge.Name);

            if (badgeLevelOfPitch == null || badgeLevelOfReviewer == null || badgeLevelOfPitch.Level > badgeLevelOfReviewer.Level)
            {
                throw new Exception("You are not allowed to give a review.");
            }
            applicationContext.Add(newReview);
            applicationContext.SaveChanges();
        }
    }
}
