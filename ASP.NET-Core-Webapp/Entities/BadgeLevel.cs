﻿using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Entities
{
    public class BadgeLevel
    {
        public int Level { get; set; }
        public string Description { get; set; }
        public ICollection<string> Holders { get; set; }
    }
}