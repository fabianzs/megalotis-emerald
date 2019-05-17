using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public interface IReviewService
    {
        void CreateReview(string openId, ReviewDTO reviewDTO);
        void UpdateReview(string openId, ReviewDTO reviewDTO, long id);
    }
}
