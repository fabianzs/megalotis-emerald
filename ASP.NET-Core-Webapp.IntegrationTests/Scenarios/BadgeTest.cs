using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    [Collection("BaseCollection")]
    public class BadgeTest
    {
        private readonly TestContext testContext;

        public BadgeTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task MyBadges_Should_Return_ReturnOk()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/mybadges");
            HttpResponseMessage response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task MyBadges_Should_Return_Badge_In_Proer_Format()
        {
            var response = await testContext.Client.GetAsync("/mybadges");
            response.EnsureSuccessStatusCode();
            Assert.Equal("{\"badges\":[{\"name\":\"test\",\"levels\":[],\"tag\":null,\"version\":null}]}",
                response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task Pitch_ContentTypeJson_Success()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/mybadges");
            var response = await testContext.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
