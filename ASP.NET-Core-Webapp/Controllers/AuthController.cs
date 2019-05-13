using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Helpers;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ApplicationContext applicationContext;

        public AuthController(IAuthService authService, ApplicationContext applicationContext)
        {
            this.authService = authService;
            this.applicationContext = applicationContext;
        }

        [HttpGet("")]
        public IActionResult Login()
        {
            return Redirect(authService.GetGoogleLogin());
        }

        [HttpGet("auth")]
        public IActionResult Authenticate(string code)
        {
            GoogleToken token = authService.GetToken(code);
            TokenInfo tokenInfo = authService.ValidateToken(token.id_token);
            bool isValid = tokenInfo.email_verified;
            if (isValid)
            {
                if(applicationContext.Users.FirstOrDefault(u => u.OpenId.Equals(tokenInfo.sub)) == null)
                {
                    User user = new User
                    {
                        Name = $"{tokenInfo.family_name} {tokenInfo.given_name}",
                        Picture = tokenInfo.picture,
                        Email = tokenInfo.email,
                        OpenId = tokenInfo.sub
                    };
                    applicationContext.Users.Add(user);
                    applicationContext.SaveChanges();
                }
                string tokenstring = authService.CreateJwtToken(tokenInfo.sub, $"{tokenInfo.family_name} {tokenInfo.given_name}", tokenInfo.email, tokenInfo.picture);
                return Ok(tokenstring);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize("Bearer")]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("You are authorized");
        }
    }
}
