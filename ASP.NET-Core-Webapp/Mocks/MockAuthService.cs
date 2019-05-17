using ASP.NET_Core_Webapp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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
            return "verysecuretokendjawuidguowa76795432";
        }
    }
}

