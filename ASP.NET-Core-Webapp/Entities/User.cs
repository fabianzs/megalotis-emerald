using System.Collections.Generic;

namespace ASP.NET_Core_Webapp.Entities
{
    public class User
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string MyProperty { get; set; }
        public string Email { get; set; }
        public string OpenId { get; set; }

        

        public User()
        {

        }
    }
}
