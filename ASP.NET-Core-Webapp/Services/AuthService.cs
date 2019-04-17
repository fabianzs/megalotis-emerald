using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{

    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;

        public AuthService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetGoogleLogin()
        {
            string base_url = "https://accounts.google.com/o/oauth2/v2/auth";
            string scope = "email+openid";
            string redirect_uri = "http://localhost:64004/auth";
            string response_type = "code";
            string client_id = configuration["Authentication:Google:ClientId"];
            return $"{base_url}?scope={scope}&redirect_uri={redirect_uri}&response_type={response_type}&client_id={client_id}";
        }
    }
}
