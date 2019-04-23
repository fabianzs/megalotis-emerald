using System.Collections.Generic;


namespace ASP.NET_Core_Webapp.Entities
{
    public class Badge
    {
        public string Name { get; set; }
        public ICollection<LevelEntity> Levels { get; set; }
        public string Tag { get; set; }
        public string Version { get; set; }

        public Badge()
        {
            this.Levels = new List<LevelEntity>();
        }

        public Badge(string name)
        {
            this.Name = name;
            this.Levels = new List<LevelEntity>();
        }
    }
}
