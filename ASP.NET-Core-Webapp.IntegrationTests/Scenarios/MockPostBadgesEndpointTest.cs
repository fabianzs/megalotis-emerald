//using ASP.NET_Core_Webapp.Entities;
//using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
//using Newtonsoft.Json;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
//{
//    /// <summary>
//    /// Tests for testing BadgeController RecieveBadge action
//    /// </summary>
//    /// 
//    [Collection("BaseCollection")]
//    public class MockPostBadgesEndpointTest
//    {
//        private readonly TestContext testContext;

//        public MockPostBadgesEndpointTest(TestContext testContext)
//        {
//            this.testContext = testContext;
//        }


//        [Fact]
//        public async Task MockPostBadges_NoBodyContent_ShouldReturn404()
//        {
//            var request = new HttpRequestMessage(HttpMethod.Post, "/badges");
//            request.Headers.Add("Authorization", "Bearer Token");
//            request.Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

//            var response = await testContext.Client.SendAsync(request);
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//        }

//        [Fact]
//        public async Task MockPostBadges_AllFielsdFilled_ShouldReturn201()
//        {
//            Badge badge = new Badge() { Name = "test", Tag = "test", Version = "test" };
//            List<string> holders = new List<string>() { "Laci", "Gábor", "Levente" };
//            BadgeLevel testlevel = new BadgeLevel() { Description = "test", Holders = holders, Level = 2 };

//            badge.Levels.Add(testlevel);

//            var request = new HttpRequestMessage(HttpMethod.Post, "/badges");
//            request.Headers.Add("Authorization", "Bearer Token");
//            request.Content = (new StringContent(JsonConvert.SerializeObject(badge), Encoding.UTF8, "application/json"));

//            var response = await testContext.Client.SendAsync(request);

//            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//        }

//        [Fact]
//        public async Task MockPostBadges_NotAuthorized_ShouldReturn401()
//        {
//            // Must work after authentication implementation
//            var request = new HttpRequestMessage(HttpMethod.Post, "/badges");
//            request.Headers.Add("Authorization", "Bearer notValidToken");

//            var response = await testContext.Client.SendAsync(request);

//            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//        }

//        [Fact]
//        public async Task MockPostBadges_NotAllFieldFilledLevelsIsMissing_ShouldReturn404WithBody()
//        {
//            var request = new HttpRequestMessage(HttpMethod.Post, "/badges");
//            request.Headers.Add("Authorization", "Bearer token");
//            request.Content = new StringContent(JsonConvert.SerializeObject(new Badge() { Name = "test", Tag = "test", Version = "test", Levels = null }), Encoding.UTF8, "application/json");

//            var response = await testContext.Client.SendAsync(request);

//            Assert.Equal("{\"error\":\"Please provide all fields\"}", response.Content.ReadAsStringAsync().Result);

//        }
//    }
//}
