﻿using ASP.NET_Core_Webapp.Data;
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
                Pitch = applicationContext.Pitches
                                            .Include(p => p.BadgeLevel)
                                            .ThenInclude(bl => bl.Badge)
                                            .FirstOrDefault(p => p.PitchId == reviewDTO.PitchId),
                User = applicationContext.Users
                                            .Include(u => u.UserLevels)
                                            .ThenInclude(ul => ul.Badgelevel)
                                            .ThenInclude(bl => bl.Badge)
                                            .FirstOrDefault(u => u.OpenId == openId)
            };

            if (newReview.Pitch == null)
            {
                throw new NullReferenceException();
            }

            BadgeLevel badgeLevelOfPitch = newReview.Pitch.BadgeLevel;
            BadgeLevel badgeLevelOfReviewer = newReview.User.UserLevels
                                                                .Select(ul => ul.Badgelevel)
                                                                .FirstOrDefault(bl => bl.Badge.Name.ToLower().Contains(badgeLevelOfPitch.Badge.Name.ToLower()));

            if (badgeLevelOfPitch == null || badgeLevelOfReviewer == null || badgeLevelOfPitch.Badge.Name != badgeLevelOfReviewer.Badge.Name || newReview.User == newReview.Pitch.User)
            {
                throw new UnauthorizedAccessException();
            }

            applicationContext.Add(newReview);
            applicationContext.SaveChanges();
        }

        public void UpdateReview(string openId, ReviewDTO reviewDTO, long id)
        {
            Review reviewToUpdate = applicationContext.Reviews.Include(r => r.User).Include(r => r.Pitch).FirstOrDefault(r => r.ReviewId == id);

            if (reviewToUpdate == null)
            {
                throw new NullReferenceException();
            }

            reviewToUpdate.Message = reviewDTO.Message;
            reviewToUpdate.Status = reviewDTO.Status;

            if (reviewToUpdate.Pitch.PitchId != reviewDTO.PitchId)
            {
                throw new InvalidOperationException();
            }

            if (reviewToUpdate.User.OpenId != openId)
            {
                throw new UnauthorizedAccessException();
            }

            applicationContext.Reviews.Update(reviewToUpdate);
            applicationContext.SaveChanges();
        }
    }
}
