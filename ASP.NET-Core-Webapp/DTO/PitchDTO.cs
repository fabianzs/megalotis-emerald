using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.DTO
{
    public class PitchDTO
    {
        public string BadgeName { get; set; }
        public string OldLVL { get; set; }
        public int PitchedLVL { get; set; }
        public string PitchMessage { get; set; }
        public virtual ICollection<String> Holders { get; set; }

        public PitchDTO()
        {
            Holders = new List<String>();
        }

        public Pitch CreatePitch(ApplicationContext applicationContext, PitchDTO pitchDTO)
        {
            if (applicationContext == null)
            {
                throw new ArgumentNullException(nameof(applicationContext));
            }

            Pitch pitch = new Pitch()
            {
                Badge = applicationContext.Badges.FirstOrDefault(b => b.Name == pitchDTO.BadgeName && b.Version == pitchDTO.OldLVL),
                PitchMessage = pitchDTO.PitchMessage,
                PitchedLevel = pitchDTO.PitchedLVL,
              
        };
            return pitch;
        }   
    }
}
