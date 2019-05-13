using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IAuthService authService;
        private readonly IReviewService reviewService;

        public ReviewController(IAuthService authService, IReviewService reviewService)
        {
            this.authService = authService;
            this.reviewService = reviewService;
        }

        [HttpPost("review")]
        public IActionResult PostReview([FromBody]ReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
            {
                return StatusCode(404, new { error = "No message body" });
            }

            if (reviewDTO.Message == null || reviewDTO.Status == null || reviewDTO.PitchId == null)
            {
                return NotFound(new { error = "Please provide all fields" });
            }

            string openId = authService.GetOpenIdFromJwtToken(Request);
            
            return Created("/review", new { message = "Success" });
        }
    }
}