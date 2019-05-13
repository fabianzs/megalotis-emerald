using ASP.NET_Core_Webapp.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public interface IAuthService
    {
        string GetGoogleLogin();
        GoogleToken GetToken(string code);
        TokenInfo ValidateToken(string id_token);
        string CreateJwtToken(string sub, string email);
        string GetOpenIdFromJwtToken(HttpRequest request);
    }
}
