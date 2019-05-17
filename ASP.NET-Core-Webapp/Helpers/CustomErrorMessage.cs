using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Helpers
{
    public class CustomErrorMessage
    {
        public string Error { get; set; }

        public CustomErrorMessage()
        {
        }

        public CustomErrorMessage(string error)
        {
            Error = error;
        }
    }
}
