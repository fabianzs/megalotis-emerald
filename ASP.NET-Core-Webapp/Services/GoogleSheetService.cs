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
    public class GoogleSheetService : IGoogleSheetService
    {
        IConfiguration configuration;
        ApplicationContext applictionContext;
        HttpClient httpClient;
        private static string StaticAccesToken;

        public string AccesToken
        {
            get { return StaticAccesToken; }
            set { StaticAccesToken = value; }
        }


        public GoogleSheetService(ApplicationContext applictionContext,IConfiguration configuration, HttpClient client)
        {
            this.applictionContext = applictionContext;
            this.configuration = configuration;
            this.httpClient = client;
        }

        public async Task FillUpDataBaseFromSpreadSheet()
        {
            SpreadSheet spreadSheet = JsonConvert.DeserializeObject<SpreadSheet>(await ReturnBadgesSpreadSheetContent());
            foreach (string[] spreadSheetBadge in spreadSheet.Values)
            {
                Badge badgeToAdd = new Badge
                { Version = spreadSheetBadge[0],
                    Name = spreadSheetBadge[1],
                    Tag = spreadSheetBadge[2] };
                applictionContext.Add(badgeToAdd);
                applictionContext.SaveChanges();
            }
        }

        public async Task<string> ReturnBadgesSpreadSheetContent()
        {
            HttpResponseMessage response = await httpClient.SendAsync(ReturnSpreadSheetRequest());
            return await response.Content.ReadAsStringAsync();
        }

        public HttpRequestMessage ReturnSpreadSheetRequest()
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
