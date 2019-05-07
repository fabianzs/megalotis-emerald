using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public class GoogleSheetService
    {
        private static string AccesToken;

        public string AccesTokenPublic
        {
            get { return AccesToken; }
            set { AccesToken = value; }
        }



    }
}
