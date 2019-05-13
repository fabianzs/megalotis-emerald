using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.DTO
{
    public class ReviewDTO
    {
        public string Message { get; set; }
        public bool? Status { get; set; }
        public int? PitchId { get; set; }

        public Review CreateReview(ApplicationContext applicationContext, string openId)
        {
            Review newReview = new Review()
            {
                Message = this.Message,
                Status = this.Status,
                Pitch = applicationContext.Pitches.FirstOrDefault(p => p.PitchId == this.PitchId),
                User = applicationContext.Users.Where(u => u.OpenId == openId).FirstOrDefault()
            };
            return newReview;
        }

        public Review UpdateReview(ApplicationContext applicationContext, string openId, long id)
        {
            Review updatedReview = new Review() { ReviewId = id, Message = this.Message, Status = this.Status, Pitch = applicationContext.Pitches.FirstOrDefault(p => p.PitchId == this.PitchId), User = applicationContext.Users.Where(u => u.OpenId == openId).FirstOrDefault() };
            return updatedReview;
        }
    }
}
