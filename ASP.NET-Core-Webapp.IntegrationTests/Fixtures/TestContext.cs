using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace ASP.NET_Core_Webapp.IntegrationTests.Fixtures
{
    public class TestContext : IDisposable
    {
        private TestServer server;
        public HttpClient Client { get; set; }

        public TestContext()
        {
            var projectDir = System.IO.Directory.GetCurrentDirectory();
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            server = new TestServer(builder);
            Client = server.CreateClient();
        }

        public void Dispose()
        {
            server.Dispose();
            Client.Dispose();
        }
    }
}
