﻿using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly ApplicationContext applicationContext;

        public BadgeService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public void CheckBadgeDTO(BadgeDTO badgeDTO)
        {
            if (badgeDTO == null)
            {
                throw new NoMessageBodyException();
            }

            if (badgeDTO.Levels == null || badgeDTO.Name == null || badgeDTO.Tag == null || badgeDTO.Version == null)
            {
                throw new MissingFieldsException();
            }
        }

        public Badge CreateBadge(BadgeDTO badgeDTO)
        {
            CheckBadgeDTO(badgeDTO);

            Badge badge = new Badge()
            {
                Name = badgeDTO.Name,
                Version = badgeDTO.Version,
                Tag = badgeDTO.Tag,
            };

            foreach (BadgeLevelDTO bl in badgeDTO.Levels)
            {
                badge.Levels.Add(CreateBadgeLevel(bl));
            }
            return badge;
        }

        public BadgeLevel CreateBadgeLevel(BadgeLevelDTO badgeLevelDTO)
        {
            BadgeLevel newBadgeLevel = new BadgeLevel()
            {
                Level = badgeLevelDTO.Level,
                Description = badgeLevelDTO.Description,
                UserLevels = new List<UserLevel>()
            };

            foreach (string holder in badgeLevelDTO.Holders)
            {
                newBadgeLevel.UserLevels.Add(CreateUserLevel(newBadgeLevel, holder));
            }
            return newBadgeLevel;
        }

        public UserLevel CreateUserLevel(BadgeLevel newBadgeLevel, string holder)
        {
            return new UserLevel()
            {
                BadgeLevel = newBadgeLevel,
                User = applicationContext.Users
                                            .Include(u => u.UserLevels)
                                            .ThenInclude(ul => ul.BadgeLevel)
                                            .ThenInclude(bl => bl.Badge)
                                            .FirstOrDefault(u => u.Name.Equals(holder))
            };
        }
    }
}
