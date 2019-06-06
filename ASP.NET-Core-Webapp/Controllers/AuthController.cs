using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Helpers;
using ASP.NET_Core_Webapp.Helpers.Exceptions;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
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
                authService.AddUserIfNotExist(tokenInfo);
                string tokenstring = authService.CreateJwtToken(tokenInfo.sub, $"{tokenInfo.family_name} {tokenInfo.given_name}", tokenInfo.email, tokenInfo.picture);
                return Ok(tokenstring);
            }
            else
            {
                throw new InvalidEmailException();
            }
        }

        [Authorize]
        [HttpGet("heartbeat")]
        public IActionResult Test()
        {
            return Ok("You are authorized");
        }
    }
}
