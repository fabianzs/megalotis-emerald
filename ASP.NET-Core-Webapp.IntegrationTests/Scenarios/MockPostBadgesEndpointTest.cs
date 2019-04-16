using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    [Collection("BaseCollection")]
    public class MockPostBadgesEndpointTest
    {
        private readonly TestContext testContext;

        public MockPostBadgesEndpointTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task MockPostBadges_Should_Return_404()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/badges?authorized=1");
            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


    }
}
