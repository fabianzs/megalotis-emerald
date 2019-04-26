using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class Pitch
    {
        public long PitchId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int PitchedLevel { get; set; }
        public string PitchMessage { get; set; }
        public User User { get; set; }
        public Badge Badge { get; set; }
        public BadgeLevel BadgeLevel { get; set; }

        public ICollection<Review> Holders { get; set; }

        public Pitch()
        {

        }

        public Pitch(string username, string badgeName, int oldLevel, int pitchedLevel, string pitchMessage, ICollection<Review> holders)
        {
            TimeStamp = DateTime.Now;
            PitchedLevel = pitchedLevel;
            PitchMessage = pitchMessage;
            Holders = holders;
        }
    }
}