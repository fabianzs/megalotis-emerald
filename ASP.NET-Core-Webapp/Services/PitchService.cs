using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Helpers.Exceptions;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_Webapp.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            Entities.Pitch newPitch = PitchDTO.CreatePitch(applicationContext, pitchDTO);
            string openId = authService.GetOpenIdFromJwtToken(request);

            Entities.User user = applicationContext.Users.Include(a => a.Pitches).ThenInclude(p => p.Badge).FirstOrDefault(u => u.OpenId == openId);
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

        public bool ModifyPitch(Entities.Pitch pitch, HttpRequest request)
        {
            string openId = authService.GetOpenIdFromJwtToken(request);
            Entities.User user = applicationContext.Users.Include(a => a.Pitches).ThenInclude(p => p.Badge).FirstOrDefault(u => u.OpenId == openId);
            List<string> badgeNames = user.Pitches.Select(p => p.Badge.Name).ToList();

            if (pitch.Badge != null && badgeNames.Contains(pitch.Badge.Name))
            {
                applicationContext.Update(pitch);
                applicationContext.SaveChanges();
                return true;
            }
            return false;
        }

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

        public bool PutPitch(Entities.Pitch pitch, HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
