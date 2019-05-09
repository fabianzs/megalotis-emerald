using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public class GoogleSheetService
    {
        IConfiguration configuration;
        ApplicationContext applictionContext;
        private static string StaticAccesToken;

        public string AccesToken
        {
            get { return StaticAccesToken; }
            set { StaticAccesToken = value; }
        }


        public GoogleSheetService(ApplicationContext applictionContext,IConfiguration configuration)
        {
            this.applictionContext = applictionContext;
            this.configuration = configuration;
        }

        public void FillUpDataBaseFromSpreadSheet()
        {
            //this.SeedObject = JsonConvert.DeserializeObject<SeedObject>(Json2);
            Badge t = JsonConvert.DeserializeObject<Badge>(GetBadgesFromStyleSheet());
           // applictionContext.Add
        }

        public string GetBadgesFromStyleSheet()
        {
            var dict = new List<KeyValuePair<string, string>>();
            dict.Add(new KeyValuePair<string, string>("Authorization", $"Bearer {StaticAccesToken}"));
            //dict.Add(new KeyValuePair<string, string>("client_id", configuration["Authentication:Google:ClientId"]));
            //dict.Add(new KeyValuePair<string, string>("client_secret", configuration["Authentication:Google:ClientSecret"]));
            //dict.Add(new KeyValuePair<string, string>("redirect_uri", configuration.GetValue<string>("AppSettings:Authentication endpoint")));
            //dict.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            //var client = new HttpClient();
            //var req = new HttpRequestMessage(HttpMethod.Post, "https://www.googleapis.com/oauth2/v4/token");
            //req.Content = new FormUrlEncodedContent(dict);
            //HttpResponseMessage response = client.SendAsync(req).Result;
            //string res = response.Content.ReadAsStringAsync().Result;
            //GoogleToken token = JsonConvert.DeserializeObject<GoogleToken>(res);
            //googleSheetService.AccesToken = token.access_token;
            //return token;
            var req = new HttpRequestMessage(HttpMethod.Get, MakeGoogleSheetApiURL());
            req.Headers.Add("Authorization", $"Bearer {StaticAccesToken}");
            HttpResponseMessage response = new HttpClient().SendAsync(req).Result;
            string res = response.Content.ReadAsStringAsync().Result;
            return res;
        }

        public string MakeGoogleSheetApiURL()
        {
            string baseURL = configuration["GoogleSheet:GoogleSheetApiBaseURL"];
            string spreadSheetID = configuration["GoogleSheet:SpreadSheetID"];
            string range = configuration["GoogleSheet:Range"];
           // return $"{baseURL}{spreadSheetID}/values/{range}?access_token={StaticAccesToken}";
            return $"{baseURL}{spreadSheetID}/values/{range}";
        }


    }
}
