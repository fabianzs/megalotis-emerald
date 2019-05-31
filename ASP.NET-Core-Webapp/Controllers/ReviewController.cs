using System;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Helpers;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize]
        [HttpPost("review")]
        public async Task<IActionResult> PostReview([FromBody]ReviewDTO reviewDTO)
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
            await reviewService.CreateReview(openId, reviewDTO);
            return Created("/review", new { message = "Success" });
        }

        [Authorize]
        [HttpPut("review/{id}")]
        public async Task<IActionResult> PutReview([FromRoute] long id, [FromBody] ReviewDTO reviewDTO)
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
            await reviewService.UpdateReview(openId, reviewDTO, id);
            return Ok(new { message = "Success" });
        }
    }
}