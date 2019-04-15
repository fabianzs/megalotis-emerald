using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class Skill
    {
        public int Level { get; set; }
        public string Description { get; set; }
        public ICollection<string> Holders { get; set; }
    }
}
