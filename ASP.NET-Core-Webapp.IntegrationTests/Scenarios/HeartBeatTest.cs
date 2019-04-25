using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using ASP.NET_Core_Webapp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Castle.Core.Configuration;
using System.Net.Http.Headers;

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
            var tokenstring = testContext.AuthService.CreateJwtToken("1234", "balogh.botond8@gmail.com");
            var request = new HttpRequestMessage(HttpMethod.Get, "/heartbeat");
            request.Headers.Authorization = new AuthenticationHeaderValue("Authorization", tokenstring);


            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
