using ASP.NET_Core_Webapp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

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
            string scope = "openid%20email%20profile";
            string redirect_uri = configuration.GetValue<string>("AppSettings:Authentication endpoint");
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
            dict.Add(new KeyValuePair<string, string>("redirect_uri", configuration.GetValue<string>("AppSettings:Authentication endpoint")));
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

        public string CreateJwtToken(string sub, string name, string email, string picture)
        {
            JwtSecurityToken token = new JwtSecurityToken(
                claims: new Claim[] { new Claim("OpenID", sub), new Claim("Name", name), new Claim("Email", email), new Claim("Picture", picture) },
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:Jwt:Secret"])), SecurityAlgorithms.HmacSha256Signature)
                );
            string securetoken = new JwtSecurityTokenHandler().WriteToken(token);

            return securetoken;
        }

        public string GetOpenIdFromJwtToken(HttpRequest request)
        {
            string tokenString = request.Headers["Authorization"];
            string token = tokenString.Split(" ")[1];
            JwtSecurityToken jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            return jwtToken.Claims.First(claim => claim.Type == "OpenID").Value;
        }
    }
}
