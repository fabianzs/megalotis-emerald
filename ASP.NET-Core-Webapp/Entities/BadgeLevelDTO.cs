using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class BadgeLevelDTO
    {
        public long BadgeLevelId { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public Badge Badge { get; set; }
        public virtual ICollection<String> Holders { get; set; }
    }
}
