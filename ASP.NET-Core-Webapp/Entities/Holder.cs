using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class Holder
    {
        public string Name { get; set; }
        public object Message { get; set; }
        public bool PitchStatus { get; set; }

        public Holder(string name, object message, bool pitchStatus)
        {
            Name = name;
            Message = message;
            PitchStatus = pitchStatus;
        }
    }

}
