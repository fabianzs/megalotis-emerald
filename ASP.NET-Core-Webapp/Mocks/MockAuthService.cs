using ASP.NET_Core_Webapp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{

    public class MockAuthService : IAuthService
    {
        private readonly IConfiguration configuration;

        public MockAuthService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetGoogleLogin()
        {
            return null;
        }

        public GoogleToken GetToken(string code)
        {
            return null;
        }

        public TokenInfo ValidateToken(string id_token)
        {
            return null;
        }

        public string CreateJwtToken(string sub, string name, string email, string picture)
        {
            return null;
        }

        public string GetOpenIdFromJwtToken(HttpRequest request)
        {
            return "111300160267154007210";
        }
    }
}
