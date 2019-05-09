using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
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
        ApplicationContext applictionContext;
        private static string StaticAccesToken;

        public string AccesToken
        {
            get { return StaticAccesToken; }
            set { StaticAccesToken = value; }
        }


        public GoogleSheetService(ApplicationContext applictionContext)
        {
            this.applictionContext = applictionContext;
        }

        public void FillUpDataBaseFromSpreadSheet()
        {
            //Json2 = StreamReader.ReadToEnd();
            //this.SeedObject = JsonConvert.DeserializeObject<SeedObject>(Json2);
            Badge t = JsonConvert.DeserializeObject<Badge>(GetBadgesFromStyleSheet());
            
        }

        public string GetBadgesFromStyleSheet()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "https://sheets.googleapis.com/v4/spreadsheets/1Qx9LO2QxXp7bB9IOSIr8NxXWVbbu-a5Ta4Igwp1i92I/values/Badges?access_token=" + StaticAccesToken);
            HttpResponseMessage response = new HttpClient().SendAsync(req).Result;
            string res = response.Content.ReadAsStringAsync().Result;
            return res;
        }
    }
}
