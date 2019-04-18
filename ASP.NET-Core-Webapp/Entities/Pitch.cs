using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class Pitch
    {
            public DateTime Timestamp { get; set; }
            public string Username { get; set; }
            public string BadgeName { get; set; }
            public int OldLevel { get; set; }
            public int PitchedLevel { get; set; }
            public string PitchMessage { get; set; }
            public ICollection<Holder> Holders { get; set; }

        public Pitch(string username, string badgeName, int oldLevel, int pitchedLevel, string pitchMessage, ICollection<Holder> holders)
        {
            Timestamp = DateTime.Now;
            Username = username;
            BadgeName = badgeName;
            OldLevel = oldLevel;
            PitchedLevel = pitchedLevel;
            PitchMessage = pitchMessage;
            Holders = holders;
        }
    }
}
