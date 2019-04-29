using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                DataBase.SaveChanges();
            }

            var userList = SeedObject.users;

            for (int i = 0; i < userList.Length; i++)
            {
                Entities.User userToAdd = new Entities.User() { Name = userList[i].name, Picture = userList[i].pic, OpenId = userList[i].tokenAuth };
                DataBase.Add(userToAdd);
                DataBase.SaveChanges();
            }

            var pitchList = SeedObject.pitches;

            for (int i = 0; i < pitchList.Length; i++)
            {
                Entities.Pitch pitchToAdd = new Entities.Pitch() { };
                
                Entities.User userToAdd = new Entities.User();
                userToAdd = DataBase.Users.Where(x => x.Name == pitchList[i].username).FirstOrDefault();
                if (userToAdd == null)
                {
                    Entities.User newUser = new Entities.User() { Name = pitchList[i].username };
                    //DataBase.Add(newUser);
                    pitchToAdd.User = newUser;
                    DataBase.SaveChanges();
                }
                else
                {
                pitchToAdd.User = userToAdd;
                DataBase.SaveChanges();
                }


                Entities.Badge badgeToAdd = new Entities.Badge();
                badgeToAdd = DataBase.Badges.Where(x => x.Name == pitchList[i].badgeName).FirstOrDefault();
                pitchToAdd.Badge = badgeToAdd;

                pitchToAdd.TimeStamp = DateTime.ParseExact(pitchList[i].timestamp, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                pitchToAdd.PitchMessage = pitchList[i].pitchMessage;

                DataBase.Add(pitchToAdd);
                DataBase.SaveChanges();
            }

            DataBase.SaveChanges();
        }
    }
}
