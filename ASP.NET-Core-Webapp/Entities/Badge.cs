using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Entities
{

    public class Badge
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public ICollection<LevelEntity> Levels { get; set; }

    }
}
