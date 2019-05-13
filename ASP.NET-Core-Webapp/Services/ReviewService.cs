using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            Review newReview = new Review() { Message = reviewDTO.Message, Status = reviewDTO.Status, Pitch = applicationContext.Pitches.FirstOrDefault(p => p.PitchId == reviewDTO.PitchId), User = applicationContext.Users.Where(u => u.OpenId == openId).FirstOrDefault() };
            applicationContext.Add(newReview);
            applicationContext.SaveChanges();
        }

        public void UpdateReview(string openId, ReviewDTO reviewDTO, long id)
        {
            Review updatedReview = new Review() { ReviewId = id, Message = reviewDTO.Message, Status = reviewDTO.Status, Pitch = applicationContext.Pitches.FirstOrDefault(p => p.PitchId == reviewDTO.PitchId), User = applicationContext.Users.Where(u => u.OpenId == openId).FirstOrDefault() };
            applicationContext.Update(updatedReview);
            applicationContext.SaveChanges();
        }
    }
}
