using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    [Collection("BaseCollection")]
    public class HeartBeatTest
    {
        private readonly TestContext testContext;

        public HeartBeatTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task HeartBeat_Authorized_ReturnOK()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "heartbeat");
            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
