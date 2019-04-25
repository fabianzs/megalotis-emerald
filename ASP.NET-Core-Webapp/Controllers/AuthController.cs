﻿using ASP.NET_Core_Webapp.Helpers;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                var tokenstring = authService.CreateJwtToken(tokenInfo.sub, $"{tokenInfo.family_name} {tokenInfo.given_name}", tokenInfo.email, tokenInfo.picture);
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
