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
            var userList = SeedObject.users;

            for (int i = 0; i < userList.Length; i++)
            {
                Entities.User userToAdd = new Entities.User() { Name = userList[i].name, Picture = userList[i].pic, OpenId = userList[i].tokenAuth };
                DataBase.Add(userToAdd);
                DataBase.SaveChanges();
            }

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

                var holders = badgesList[i].levels;
                foreach (var item in holders)
                {
                    foreach (var user in item.holders)
                    {
                        Entities.User userToAddFromBadges = new Entities.User();
                        userToAddFromBadges.Name = user;
                        if (DataBase.Users.Where(x => x.Name == user).FirstOrDefault() == null)
                        {
                            DataBase.Add(userToAddFromBadges);
                            DataBase.SaveChanges();
                        }
                        DataBase.SaveChanges();
                    }
                }


            }

            DataBase.SaveChanges();

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

                // Add reviews:
                List<Review> reviews = new List<Review>();
                foreach (var holderReview in pitchList[i].holders)
                {
                    Review review = new Review(holderReview.message, holderReview.pitchStatus);

                    reviews.Add(review);
                    DataBase.SaveChanges();

                    review.User = DataBase.Users.Where(x => x.Name == holderReview.name).FirstOrDefault();
                    review.Pitch = DataBase.Pitches.Where(x => x.PitchMessage == holderReview.message).FirstOrDefault();

                    DataBase.Reviews.Add(review);
                    DataBase.SaveChanges();
                }

                DataBase.SaveChanges();



                var badgesList2 = SeedObject.library;

                foreach (var badge in badgesList2)
                {
                    foreach (var level in badge.levels)
                    {
                        foreach (var user in level.holders)
                        {
                            UserLevel userLevel = new UserLevel();

                            BadgeLevel badgeLevelNew = DataBase.BadgeLevels.Where(x => x.Description == level.description).FirstOrDefault();

                            userLevel.User = DataBase.Users.Where(x => x.Name == user).FirstOrDefault();
                            userLevel.Badgelevel = badgeLevelNew;

                            if (DataBase.UserLevels.Count(x => x.BadgeLevelId == userLevel.Badgelevel.BadgeLevelId && x.UserId == userLevel.User.UserId) == 0)
                            {
                                DataBase.Add(userLevel);
                                DataBase.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}
