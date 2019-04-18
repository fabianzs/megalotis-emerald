
﻿using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Entities
{
    public class LevelEntity
    {
        public int Level { get; set; }
        public string Description { get; set; }
        public ICollection<string> Holders { get; set; }
    }
}