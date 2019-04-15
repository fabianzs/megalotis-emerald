using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class Badge
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public Badge(string name, int level)
        {
            this.Name = name;
            this.Level = level;
        }
    }
}
