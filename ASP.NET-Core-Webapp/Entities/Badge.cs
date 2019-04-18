<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class Badge
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string Tag { get; set; }
        public string Version { get; set; }
        

        public Badge(string name, int level)
        {
            this.Name = name;
            this.Level = level;
        }
    }
}
=======
﻿using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Entities
{

    public class Badge
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public ICollection<LevelEntity> Levels { get; set; }

        public Badge()
        {
            this.Levels = new List<LevelEntity>();
        }

    }
}
>>>>>>> origin/Mock_Post_Badges_Endpoint
