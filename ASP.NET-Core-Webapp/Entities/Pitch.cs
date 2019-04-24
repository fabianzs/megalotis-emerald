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
<<<<<<< HEAD
        public List<string> Holders { get; set; }

        public Pitch()
=======
        public ICollection<Review> Holders { get; set; }

        public Pitch(string username, string badgeName, int oldLevel, int pitchedLevel, string pitchMessage, ICollection<Review> holders)
>>>>>>> 8619cfc... Rename Holder class to Review and fix fields
        {
            
        }
    }


}
