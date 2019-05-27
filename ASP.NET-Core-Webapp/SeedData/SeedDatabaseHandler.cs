using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASP.NET_Core_Webapp.SeedData
{
    public class SeedDatabaseHandler
    {
        public ApplicationContext ApplicationContext { get; set; }
        public string Json2 { get; set; }
        public StreamReader StreamReader { get; set; }
        public ApplicationContext DataBase { get; set; }
        public SeedObject SeedObject { get; set; }
        public IConfiguration Configuration { get; set; }

        public SeedDatabaseHandler(ApplicationContext applicationContext, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.DataBase = applicationContext;
            this.StreamReader = new StreamReader(Configuration["SeedLocation:FileLocation"]);
            Json2 = StreamReader.ReadToEnd();
            this.SeedObject = JsonConvert.DeserializeObject<SeedObject>(Json2);
        }

        public SeedDatabaseHandler()
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
                Entities.User userToAdd = new Entities.User()
                {
                    Name = userList[i].name,
                    Picture = userList[i].pic,
                    OpenId = userList[i].tokenAuth,
                    Email =userList[i].email
                };
                
                DataBase.Add(userToAdd);
                DataBase.SaveChanges();
            }

            var badgesList = SeedObject.library;
            for (int i = 0; i < badgesList.Length; i++)
            {
                List<BadgeLevel> levelList = new List<BadgeLevel>();
                foreach (var item in badgesList[i].levels)
                {
                    BadgeLevel badgeLevel = new BadgeLevel()
                    {
                        Level = item.level,
                        Description =$"Lvl{item.level}:{item.description}"
                    };
                    levelList.Add(badgeLevel);
                }

                Entities.Badge badgeToAdd = new Entities.Badge()
                { 
                    Name = badgesList[i].name,
                    Tag = badgesList[i].tag,
                    Version = badgesList[i].version,
                    Levels = levelList
                };
                DataBase.Add(badgeToAdd);
                DataBase.SaveChanges();

                var holders = badgesList[i].levels;
                foreach (var item in holders)
                {
                    foreach (var user in item.holders)
                    {
                        Entities.User userToAddFromBadges = new Entities.User
                        {
                            Name = user
                        };

                        if (DataBase.Users.Where(u => u.Name == user).FirstOrDefault() == null)
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
                // Add user:
                Entities.User userToAdd = DataBase.Users.Where(x => x.Name == pitchList[i].username).FirstOrDefault();

                if (userToAdd == null)
                {
                    userToAdd = new Entities.User() { Name = pitchList[i].username };
                    DataBase.Add(userToAdd);
                }
                pitchToAdd.User = userToAdd;
                DataBase.Add(pitchToAdd);
                DataBase.SaveChanges();

                Entities.Badge badgeToAdd;
                badgeToAdd = DataBase.Badges
                    .Include(b => b.Levels)
                    .Where(b => b.Name.ToLower().Contains(pitchList[i].badgeName.ToLower())).FirstOrDefault();

                if (badgeToAdd == null)
                {
                    badgeToAdd = new Entities.Badge() { Name = pitchList[i].badgeName };
                    DataBase.Badges.Add(badgeToAdd);
                }

                pitchToAdd.TimeStamp = DateTime.ParseExact(pitchList[i].timestamp, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                pitchToAdd.PitchMessage = pitchList[i].pitchMessage;
                pitchToAdd.PitchedLevel = Int32.Parse(pitchList[i].pitchedLevel);
                pitchToAdd.Badge = badgeToAdd;

                Entities.BadgeLevel oldBadgeLevel = DataBase.BadgeLevels
                    .Include(bl => bl.Badge).Where(bl => bl.Badge.Name.ToLower()
                    .Contains(pitchList[i].badgeName.ToLower()) && bl.Level == Int32.Parse(pitchList[i].oldLevel)).FirstOrDefault();
                if (oldBadgeLevel == null)
                {
                    oldBadgeLevel = new Entities.BadgeLevel() { Level = Int32.Parse(pitchList[i].oldLevel) };
                    oldBadgeLevel.Badge = badgeToAdd;
                    UserLevel newUserLevel = new UserLevel() { User = userToAdd, BadgeLevel = oldBadgeLevel };
                    userToAdd.UserLevels.Add(newUserLevel);
                    DataBase.Add(oldBadgeLevel);
                    DataBase.Update(userToAdd);
                }
                pitchToAdd.BadgeLevel = oldBadgeLevel;

                // Add reviews:
                foreach (var holderReview in pitchList[i].holders)
                {
                    Review review = new Review(holderReview.message, holderReview.pitchStatus);
                    pitchToAdd.Holders.Add(review);
                    //DataBase.SaveChanges();
                    Entities.User reviewerUser = DataBase.Users.Include(u => u.UserLevels)
                        .ThenInclude(ul => ul.BadgeLevel)
                        .ThenInclude(bl => bl.Badge)
                        .Where(u => u.Name == holderReview.name).FirstOrDefault();

                    if (reviewerUser.UserLevels.Where(ul => ul.BadgeLevel.Badge.Name.ToLower()
                    .Contains(badgeToAdd.Name.ToLower())).FirstOrDefault() == null)
                    {
                        Entities.BadgeLevel newBadgeLevel = new BadgeLevel() { Level = 0, Badge = badgeToAdd };
                        UserLevel reviewerLevel = new UserLevel() { User = reviewerUser, BadgeLevel = newBadgeLevel };
                        reviewerUser.UserLevels.Add(reviewerLevel);
                    }
                    review.User = reviewerUser;
                    review.Pitch = pitchToAdd;

                    DataBase.Add(review);
                }
                userToAdd.Pitches.Add(pitchToAdd);
                pitchToAdd.User = userToAdd;
                DataBase.Update(pitchToAdd);
                DataBase.SaveChanges();

                var badgesList2 = SeedObject.library;

                foreach (var badge in badgesList2)
                {
                    foreach (var level in badge.levels)
                    {
                        foreach (var user in level.holders)
                        {
                            UserLevel userLevel = new UserLevel();

                            BadgeLevel badgeLevelNew = DataBase.BadgeLevels
                                .Where(x => x.Description == $"Lvl{level.level}:{level.description}").FirstOrDefault();

                            userLevel.User = DataBase.Users.Where(x => x.Name == user).FirstOrDefault();
                            userLevel.BadgeLevel = badgeLevelNew;

                            if (DataBase.UserLevels.Count(x => x.BadgeLevelId == userLevel.BadgeLevel.BadgeLevelId && x.UserId == userLevel.User.UserId) == 0)
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
