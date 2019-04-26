using ASP.NET_Core_Webapp.Helpers;
using Microsoft.AspNetCore.Http;

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
