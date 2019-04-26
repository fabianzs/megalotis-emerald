
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    public class PitchController : Controller
    {
        ApplicationContext app;
        private readonly IAuthService authService;

        public PitchController(ApplicationContext app, IAuthService authService)
        {
            this.app = app;
            this.authService = authService;
        }

        [Authorize("Bearer")]
        [HttpGet("pitches")]
        public Object GetPitch(HttpRequest request)
        {
            var openID = authService.GetOpenIdFromJwtToken(request);
            //var pitch = app.Users
            return StatusCode(401, new { error = "Unauthorizied" });
        }

        [HttpPost("pitches")]
        public IActionResult CreateNewPitch(Pitch newPitch)
        {
                return Created("", new { message = "Success" });
        }
    }
}