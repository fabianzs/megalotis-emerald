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
                var badegeToUpdate = applictionContext.Badges
                  .FirstOrDefault(badge =>
                  badge.Name.Equals(spreadSheetBadge[1], StringComparison.InvariantCultureIgnoreCase));

                if (badegeToUpdate == null)
                {
                    CreateBadge(spreadSheetBadge);
                }
                else
                {
                    UpdateBadge(spreadSheetBadge, badegeToUpdate);
                }
            }
        }

        public void CreateBadge(string[] badgeFields)
        {
            Badge badgeToAdd = MakeBadgeFromArrayOfFields(badgeFields);
            applictionContext.Add(badgeToAdd);
            applictionContext.SaveChanges();
        }

        public void UpdateBadge(string[] badgeFields, Badge badgeToUpdate)
        {
            Badge newBadge = MakeBadgeFromArrayOfFields(badgeFields);
            badgeToUpdate.Levels = newBadge.Levels;
            badgeToUpdate.Version = newBadge.Version;
            badgeToUpdate.Tag = newBadge.Tag;
            applictionContext.SaveChanges();
        }

        public Badge MakeBadgeFromArrayOfFields(string[] badgeFields)
        {
            return new Badge
            {
                Version = badgeFields[0],
                Name = badgeFields[1],
                Tag = badgeFields[2],
                Levels = FillUpBadgeLevelList(badgeFields)
            };
        }

        public List<BadgeLevel> FillUpBadgeLevelList(string[] spreadSheetBadge)
        {
            List<BadgeLevel> levels = new List<BadgeLevel>();
            for (int actualColumn = 3; actualColumn < spreadSheetBadge.Length; actualColumn++)
            {
                var badgeLevel = applictionContext.BadgeLevels
                    .FirstOrDefault(p =>
                    spreadSheetBadge[actualColumn].Replace(" ", "")
                    .Equals(p.Description, StringComparison.InvariantCultureIgnoreCase));

                if (badgeLevel == null)
                {
                    badgeLevel = new BadgeLevel
                    {
                        Description = spreadSheetBadge[actualColumn],
                        Level = levels.Count()
                    };
                }
                levels.Add(badgeLevel);
            }
            return levels;
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
            string baseURL =  configuration["GoogleSheet:GoogleSheetApiBaseURL"];
            string spreadSheetID = configuration["GoogleSheet:SpreadSheetID"];
            string range = configuration["GoogleSheet:Range"];
            return $"{baseURL}{spreadSheetID}/values/{range}?key=AIzaSyCEyuQolfW1jsc1RQDrxvTRH8YIaLTQsho";
        }


    }
}