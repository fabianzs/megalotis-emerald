using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASP.NET_Core_Webapp.Controllers
{
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
        public IActionResult GetPitch()
        {
            string openId = authService.GetOpenIdFromJwtToken(Request);
            var user = app.Users.Include(u => u.Pitches).FirstOrDefault(u => u.OpenId == openId);
            var pitches = user.Pitches.Select(x => new { x.PitchId, x.TimeStamp, x.PitchedLevel, x.PitchMessage }).ToList();
            return Accepted(pitches);
        }

        [HttpPost("pitches")]
        public IActionResult CreateNewPitch(Pitch newPitch)
        {
                return Created("", new { message = "Success" });
        }
    }
}

