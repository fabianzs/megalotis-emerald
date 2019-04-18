using ASP.NET_Core_Webapp.Helpers;
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

    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly AppSettings appSettings;

        public AuthService(IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            this.configuration = configuration;
            this.appSettings = appSettings.Value;
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

        public GoogleToken GetToken(string code)
        {
            var dict = new List<KeyValuePair<string, string>>();
            dict.Add(new KeyValuePair<string, string>("code", code));
            dict.Add(new KeyValuePair<string, string>("client_id", configuration["Authentication:Google:ClientId"]));
            dict.Add(new KeyValuePair<string, string>("client_secret", configuration["Authentication:Google:ClientSecret"]));
            dict.Add(new KeyValuePair<string, string>("redirect_uri", "http://localhost:64004/auth"));
            dict.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, "https://www.googleapis.com/oauth2/v4/token");
            req.Content = new FormUrlEncodedContent(dict);
            HttpResponseMessage response = client.SendAsync(req).Result;
            string res = response.Content.ReadAsStringAsync().Result;
            GoogleToken token = JsonConvert.DeserializeObject<GoogleToken>(res);
            return token;
        }

        public TokenInfo ValidateToken(string id_token)
        {
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Get, "https://oauth2.googleapis.com/tokeninfo?id_token=" + id_token);
            HttpResponseMessage response = client.SendAsync(req).Result;
            string res = response.Content.ReadAsStringAsync().Result;
            TokenInfo tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(res);
            return tokenInfo;
        }

        public string CreateJwtToken(string sub, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.NameIdentifier, sub),
                     new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var securetoken = tokenHandler.WriteToken(token);
            return securetoken;
        }
    }
}
