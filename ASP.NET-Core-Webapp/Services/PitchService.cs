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

    }
}
