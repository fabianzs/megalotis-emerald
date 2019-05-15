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
    public class MockGoogleSpreadSheetService : IGoogleSheetService
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


        public MockGoogleSpreadSheetService(ApplicationContext applictionContext, IConfiguration configuration, HttpClient client)
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
                {
                    Version = spreadSheetBadge[0],
                    Name = spreadSheetBadge[1],
                    Tag = spreadSheetBadge[2]
                };
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
            return request;
        }

        public string MakeGoogleSheetApiURL()
        {
            string baseURL = "https://sheets.googleapis.com/v4/spreadsheets/";
            string spreadSheetID = "1l6Jw7yXJVLdNjfgHMKK45HQtgHLdp1UmpxrOCiIti40";
            string range = "A2:N80";
            return $"{baseURL}{spreadSheetID}/values/{range}?key=AIzaSyCEyuQolfW1jsc1RQDrxvTRH8YIaLTQsho";
        }


    }
}