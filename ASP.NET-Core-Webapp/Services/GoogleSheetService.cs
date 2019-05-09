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
            //Json2 = StreamReader.ReadToEnd();
            //this.SeedObject = JsonConvert.DeserializeObject<SeedObject>(Json2);
            Badge t = JsonConvert.DeserializeObject<Badge>(GetBadgesFromStyleSheet());
            
        }

        public string GetBadgesFromStyleSheet()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, MakeGoogleSheetApiURL());
            HttpResponseMessage response = new HttpClient().SendAsync(req).Result;
            string res = response.Content.ReadAsStringAsync().Result;
            return res;
        }

        public string MakeGoogleSheetApiURL()
        {
            string baseURL = configuration["GoogleSheet:GoogleSheetApiBaseURL"];
            string spreadSheetID = configuration["GoogleSheet:SpreadSheetID"];
            string range = configuration["GoogleSheet:Range"];
            return $"{baseURL}{spreadSheetID}/values/{range}?access_token={StaticAccesToken}";
        }
    }
}
