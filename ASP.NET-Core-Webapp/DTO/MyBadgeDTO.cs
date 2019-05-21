using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.DTO
{
    public class MyBadgeDTO
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public MyBadgeDTO(string name, int level)
        {
            Name = name;
            Level = level;
        }
    }
}
