using ASP.NET_Core_Webapp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public class GoogleSheetService
    {

        public void FillUpDataBaseFromSpreadSheet()
        {

        }

        public string GetBadgesFromStyleSheet()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "https://sheets.googleapis.com/v4/spreadsheets/1Qx9LO2QxXp7bB9IOSIr8NxXWVbbu-a5Ta4Igwp1i92I/values/Badges?access_token=" + ReadAccesTokenFromFile());
            HttpResponseMessage response = new HttpClient().SendAsync(req).Result;
            string res = response.Content.ReadAsStringAsync().Result;
            return res;
        }

        private string ReadAccesTokenFromFile()
        {
            return System.IO.File.ReadAllText("AccesToken.txt");
        }
    }
}
