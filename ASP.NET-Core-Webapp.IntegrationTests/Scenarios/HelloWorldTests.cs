using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    [Collection("BaseCollection")]
    public class HelloWorldTests
    {
        private readonly TestContext testContext;
        
        [Fact]
        public async Task Hello_Should_ReturnOK()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/hello");
            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Hello_Should_Return_HelloWorld()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/hello");
            var response = await testContext.Client.SendAsync(request);
            Assert.Equal("Hello World!", await response.Content.ReadAsStringAsync());
        }
    }
}
