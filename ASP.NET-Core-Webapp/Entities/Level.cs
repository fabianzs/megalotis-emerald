using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class Level
    {
        public int LevelId { get; set; }
        public string Description { get; set; }
        public string[] Holders { get; set; }
    }
}
