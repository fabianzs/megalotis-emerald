using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.SeedData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public class PitchService : IPitchService
    {
        public ApplicationContext database;
        public PitchService(ApplicationContext ac)
        {
            this.database = ac;
        }

        public IList<string> CreateEmailListFromPostedPitch(SeedData.Pitch pitch)
        {
            List<Holder> holderList = new List<Holder>();
            List<string> emailList = new List<string>();
            string emailToAdd = String.Empty;
            foreach (var holder in pitch.holders)
            {
                emailToAdd = database.Users.FirstOrDefault(x => x.Name == holder.name).Email;
                if (emailToAdd != null)
                {
                    emailList.Add(emailToAdd);
                }
            }
            return emailList;
        }

        public IList<string> CreateListWithBadgeHolders(SeedData.Pitch pitch)
        {

            var usersWithBadges3 = database.Users
                .Include(x => x.UserLevels)
                .ThenInclude(x => x.BadgeLevel)
                .ThenInclude(x => x.Badge)
                .Where(x => x.UserLevels.Where(y => y.BadgeLevel.Badge.Name == pitch.badgeName) != null)
                .Where(x => x.Email != null)
                .Select(x => x.Email)
                .ToList();

            return usersWithBadges3;
        }

    }
}
