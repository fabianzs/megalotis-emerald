using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public async Task MockPostBadges_LevelsFieldMissing_ShouldReturn404()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/badges?authorized=1");

            var response = await testContext.Client.PostAsync("/badges?authorized=1", new StringContent(JsonConvert.SerializeObject(new Badge() { Name = "test", Tag = "test", Version = "test", Levels = null }), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task MockPostBadges_NoBodyContent_ShouldReturn404()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/badges?authorized=1");

            var response = await testContext.Client.PostAsync("/badges?authorized=1", new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task MockPostBadges_AllFielsdFilled_ShouldReturn201()
        {

            List<string> holders = new List<string>() { "Laci", "Gábor", "Levente" };

            Badge badge = new Badge() { Name = "test", Tag = "test", Version = "test" };

            LevelEntity testlevel = new LevelEntity() { Description = "test", Holders = holders, Level = 2 };

            badge.Levels.Add(testlevel);

            var response = await testContext.Client.PostAsync("/badges?authorized=1", new StringContent(JsonConvert.SerializeObject(badge), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task MockPostBadges_NotAuthorizet_ShouldReturn401()
        {
            List<string> holders = new List<string>() { "Laci", "Gábor", "Levente" };

            Badge badge = new Badge() { Name = "test", Tag = "test", Version = "test" };

            LevelEntity testlevel = new LevelEntity() { Description = "test", Holders = holders, Level = 2 };

            badge.Levels.Add(testlevel);

            var response = await testContext.Client.PostAsync("/badges?authorized=0", new StringContent(JsonConvert.SerializeObject(badge), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task MockPostBadges_AllFieldFilled_ShouldSuccesInBody()
        {
            List<string> holders = new List<string>() { "Laci", "Gábor", "Levente" };

            Badge badge = new Badge() { Name = "test", Tag = "test", Version = "test" };

            LevelEntity testlevel = new LevelEntity() { Description = "test", Holders = holders, Level = 2 };

            badge.Levels.Add(testlevel);

            var response = await testContext.Client.PostAsync("/badges?authorized=1", new StringContent(JsonConvert.SerializeObject(badge), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
