using ASP.NET_Core_Webapp.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public class MockSlackService : ISlackService
    {
        public MockSlackService()
        {
        }

        public async Task SendEmail(string email, string messageToSend)
        {
        }
    }
}
