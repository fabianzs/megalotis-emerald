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
            SpreadSheet spreadSheet = JsonConvert.DeserializeObject<SpreadSheet>(GetBadgesFromStyleSheet());
            foreach (string[] spreadSheetBadge in spreadSheet.Values)
            {
                //Entities.User userToAdd = new Entities.User() { Name = userList[i].name, Picture = userList[i].pic, OpenId = userList[i].tokenAuth };
                //DataBase.Add(userToAdd);
                //DataBase.SaveChanges();
                Badge badgeToAdd = new Badge { Version = spreadSheetBadge[0], Name = spreadSheetBadge[1], Tag = spreadSheetBadge[2] };
                applictionContext.Add(badgeToAdd);
                applictionContext.SaveChanges();
            }
            Console.WriteLine(string.Join(",,",applictionContext.Badges.Where(b => b.Name.Equals("test")).ToList()));
        }

        public string GetBadgesFromStyleSheet()
        {
            var dict = new List<KeyValuePair<string, string>>();
            dict.Add(new KeyValuePair<string, string>("Authorization", $"Bearer {StaticAccesToken}"));
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
            return $"{baseURL}{spreadSheetID}/values/{range}";
        }


    }
}
