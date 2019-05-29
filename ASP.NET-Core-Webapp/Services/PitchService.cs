using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Helpers.Exceptions;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ASP.NET_Core_Webapp.Services
{
    public class PitchService : IPitchService
    {
        private readonly ApplicationContext applicationContext;
        private readonly IAuthService authService;

        public PitchService(ApplicationContext applicationContext, IAuthService authService)
        {
            this.applicationContext = applicationContext;
            this.authService = authService;
        }

        public bool PostPitch(HttpRequest request,PitchDTO pitchDTO)
        {
            if (pitchDTO==null)
            {
                throw new PitchIsNullException();
            }
            Pitch newPitch = PitchDTO.CreatePitch(applicationContext, pitchDTO);
            string openId = authService.GetOpenIdFromJwtToken(request);

            User user = applicationContext.Users.Include(a => a.Pitches).ThenInclude(p => p.Badge).FirstOrDefault(u => u.OpenId == openId);
            List<string> badgeNames = user.Pitches.Select(p => p.Badge.Name).ToList();

            if (newPitch.Badge != null && !badgeNames.Contains(newPitch.Badge.Name))
            {
                newPitch.User = user;
                applicationContext.Add(newPitch);
                applicationContext.SaveChanges();
                return true;
            }
            return false;
        }

        public void ModifyPitch(string openId, long id, PitchDTO pitchDTO)
        {
            User user = applicationContext.Users.Include(a => a.Pitches).ThenInclude(p => p.Badge).FirstOrDefault(u => u.OpenId == openId);
            Pitch pitchToUpdate = user.Pitches.FirstOrDefault(p=>p.PitchId == id);

            if (pitchToUpdate == null)
            {
                throw new PitchIsNullException();
            }

            pitchToUpdate.Badge = applicationContext.Badges.FirstOrDefault(b => b.Name == pitchDTO.BadgeName);
            pitchToUpdate.BadgeLevel = applicationContext.BadgeLevels.FirstOrDefault(b => b.Level == pitchDTO.OldLVL);
            pitchToUpdate.PitchMessage = pitchDTO.PitchMessage;
            pitchToUpdate.PitchedLevel = pitchDTO.PitchedLVL; 

            if (pitchToUpdate.User.OpenId != openId)
            {
                throw new OtherUsersPitchException();
            }

            applicationContext.Pitches.Update(pitchToUpdate);
            applicationContext.SaveChanges();
        }

        public ApplicationContext database;
        public PitchService(ApplicationContext ac)
        {
            this.database = ac;
        }

        public IList<string> CreateEmailListFromPostedPitch(PitchDTO pitchDTO)
        {
            List<Review> holderList = applicationContext.Users.Include(u => u.Reviews).SelectMany(u => u.Reviews).ToList();
            List<string> emailList = new List<string>();
            for (int i = 0; i < pitchDTO.Holders.Count; i++)
            {
                foreach (Review holder in holderList)
                {
                    if (pitchDTO.Holders.Contains(holder.User.Name))
                    {
                        emailList.Add(holder.User.Email);
                    }
                }
            }
            return emailList;
        }

        public bool PutPitch(Entities.Pitch pitch, HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public void ModifyPitch(string openId, long id)
        {
            throw new NotImplementedException();
        }

        public IList<string> CreateEmailListFromPostedPitch(SeedData.Pitch pitch, PitchDTO pitchDTO)
        {
            throw new NotImplementedException();
        }

        public IList<string> CreateEmailListFromPostedPitch(SeedData.Pitch pitch)
        {
            throw new NotImplementedException();
        }
    }
}
