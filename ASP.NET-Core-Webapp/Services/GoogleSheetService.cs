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
            SpreadSheet spreadSheet = JsonConvert.DeserializeObject<SpreadSheet>(ReturnBadgesSpreadSheetContent());
            foreach (string[] spreadSheetBadge in spreadSheet.Values)
            {
                Badge badgeToAdd = new Badge { Version = spreadSheetBadge[0], Name = spreadSheetBadge[1], Tag = spreadSheetBadge[2] };
                applictionContext.Add(badgeToAdd);
                applictionContext.SaveChanges();
            }
        }

        public string ReturnBadgesSpreadSheetContent()
        {
            HttpResponseMessage response = new HttpClient().SendAsync(MakeSpreadSheetRequest()).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        public HttpRequestMessage MakeSpreadSheetRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, MakeGoogleSheetApiURL());
            request.Headers.Add("Authorization", $"Bearer {StaticAccesToken}");
            return request;
        }

        public string MakeGoogleSheetApiURL()
        {
            string baseURL = configuration["GoogleSheet:GoogleSheetApiBaseURL"];
            string spreadSheetID = configuration["GoogleSheet:SpreadSheetID"];
            string range = configuration["GoogleSheet:Range"];
            return $"{baseURL}{spreadSheetID}/values/{range}";
        }


    }
}
