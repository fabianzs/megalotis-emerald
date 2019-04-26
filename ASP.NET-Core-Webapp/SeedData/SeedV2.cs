using ASP.NET_Core_Webapp.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ASP.NET_Core_Webapp.SeedData
{
    public class SeedV2
    {
        public ApplicationContext ApplicationContext { get; set; }
        public string Json { get; set; }
        public string Json2 { get; set; }
        public StreamReader StreamReader { get; set; }
        public ApplicationContext DataBase { get; set; }
        public SeedObject SeedObject { get; set; }
        public IConfiguration Configuration { get; set; }

        public SeedV2(ApplicationContext applicationContext, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.DataBase = applicationContext;
            this.StreamReader = new StreamReader(Configuration["SeedLocation:FileLocation"]);
            Json2 = StreamReader.ReadToEnd();
            this.SeedObject = JsonConvert.DeserializeObject<SeedObject>(Json2);
        }

        public SeedV2()
        {
            this.StreamReader = new StreamReader(Configuration["SeedLocation:FileLocation"]);
            Json2 = StreamReader.ReadToEnd();
            this.SeedObject = JsonConvert.DeserializeObject<SeedObject>(Json2);
        }


        public void FillDatabaseFromObject()
        {
            var badgesList = SeedObject.library;
            for (int i = 0; i < badgesList.Length; i++)
            {
                List<BadgeLevel> levelList = new List<BadgeLevel>();
                foreach (var item in badgesList[i].levels)
                {
                    BadgeLevel badgeLevel = new BadgeLevel() { Description = item.description, Level = item.level };
                    levelList.Add(badgeLevel);
                }
                Entities.Badge badgeToAdd = new Entities.Badge() { Name = badgesList[i].name, Tag = badgesList[i].tag, Version = badgesList[i].version, Levels = levelList };

                DataBase.Add(badgeToAdd);
            }

            var userList = SeedObject.users;

            for (int i = 0; i < userList.Length; i++)
            {
                Entities.User userToAdd = new Entities.User() { Name = userList[i].name, Picture = userList[i].pic, OpenId = userList[i].tokenAuth };
                DataBase.Add(userToAdd);
            }

            //var pitchList = SeedObject.pitches;

            //for (int i = 0; i < pitchList.Length; i++)
            //{
            //    BadgeLevel badgeLevel = new BadgeLevel() { };
            //    Entities.Pitch pitchToAdd = new Entities.Pitch() { };

            //    pitchToAdd.Badge = new Entities.Badge() { Name = pitchList[i].badgeName };

            //    DataBase.Add(pitchToAdd);
            //}

            DataBase.SaveChanges();
        }
    }
}
