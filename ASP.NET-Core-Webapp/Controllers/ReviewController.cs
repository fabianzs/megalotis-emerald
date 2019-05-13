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
        private readonly ApplicationContext applicationContext;

        public ReviewController(IAuthService authService, ApplicationContext applicationContext)
        {
            this.authService = authService;
            this.applicationContext = applicationContext;
        }

        [HttpPost("review")]
        public IActionResult PostReview([FromBody] ReviewDTO reviewDTO)
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
            applicationContext.Add(reviewDTO.CreateReview(applicationContext, openId));
            applicationContext.SaveChanges();
            return Created("/review", new { message = "Success" });
        }

        [HttpPut("review/{id}")]
        public IActionResult PutReview([FromRoute] long id, [FromBody] ReviewDTO reviewDTO)
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
            applicationContext.Update(reviewDTO.UpdateReview(applicationContext, openId, id));
            applicationContext.SaveChanges();
            return Created("/review", new { message = "Success" });
        }
    }
}