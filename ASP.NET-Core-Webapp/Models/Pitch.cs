using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Models
{
    public class Pitch
    {

        public class Rootobject
        {
            public string Timestamp { get; set; }
            public string Username { get; set; }
            public string BadgeName { get; set; }
            public int OldLevel { get; set; }
            public int PitchedLevel { get; set; }
            public string PitchMessage { get; set; }
            public Holder[] Holders { get; set; }
        }

        public class Holder
        {
            public string Name { get; set; }
            public object Message { get; set; }
            public bool PitchStatus { get; set; }
        }

    }
}
