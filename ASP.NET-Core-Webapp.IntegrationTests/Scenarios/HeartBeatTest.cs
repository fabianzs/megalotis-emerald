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
        //private IUSD_CLP_ExchangeRateFeed prvGetMockExchangeRateFeed()
        //{
        //    Mock<IUSD_CLP_ExchangeRateFeed> mockObject = new Mock<IUSD_CLP_ExchangeRateFeed>();
        //    mockObject.Setup(m => m.GetActualUSDValue()).Returns(500);
        //    return mockObject.Object;
        //}

        //private IAuthService AuthService()
        //{
        //    Mock<IAuthService> mockObject = new Mock<IAuthService>();
        //    return mockObject.Object;
        //}

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

            //var response = await testContext.Client.GetAsync("heartbeat",);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.True(true);0
        }
    }
}
