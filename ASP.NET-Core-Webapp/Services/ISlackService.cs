using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public interface ISlackService
    {
        Task SendEmail(string email, string messageToSend);
    }
}
