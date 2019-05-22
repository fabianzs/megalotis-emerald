using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    /// <summary>
    /// Tests for testing BadgeController RecieveBadge action
    /// </summary>
    /// 
    [Collection("BaseCollection")]
    public class PostBadgesEndpointTest
    {
        private readonly TestContext testContext;

        public PostBadgesEndpointTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task PostBadges_NoBodyContent_ShouldReturn404()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/badges");
            request.Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostBadges_AllFielsdFilled_ShouldReturn201()
        {
            BadgeDTO badge = new BadgeDTO() { Name = "test", Tag = "test", Version = "test" };
            List<String> holders = new List<String>() { "Osztertág Szabolcs", "Zsófia Eszter Fábián" };
            BadgeLevelDTO testlevel = new BadgeLevelDTO() { Description = "test" };
            testlevel.Holders = holders;                                                                                                   
            badge.Levels.Add(testlevel);

            var request = new HttpRequestMessage(HttpMethod.Post, "/badges");
            request.Content = (new StringContent(JsonConvert.SerializeObject(badge), Encoding.UTF8, "application/json"));

            var response = await testContext.Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostBadgesMock_NoBodyContent_ShouldReturn404()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/badgesmock");
            request.Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostBadgesMock_AllFielsdFilled_ShouldReturn201()
        {
            Badge badge = new Badge() { Name = "test", Tag = "test", Version = "test" };
            List<string> holders = new List<string>() { "Laci", "Gábor", "Levente" };
            BadgeLevel testlevel = new BadgeLevel() { Description = "test" };

            badge.Levels.Add(testlevel);

            var request = new HttpRequestMessage(HttpMethod.Post, "/badgesmock");
            request.Content = (new StringContent(JsonConvert.SerializeObject(badge), Encoding.UTF8, "application/json"));

            var response = await testContext.Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task MockPostBadges_NotAllFieldFilledLevelsIsMissing_ShouldReturn404WithBody()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/badgesmock");
            request.Content = new StringContent(JsonConvert.SerializeObject(new Badge() { Name = "test", Tag = "test", Version = "test", Levels = null }), Encoding.UTF8, "application/json");

            var response = await testContext.Client.SendAsync(request);

            Assert.Equal("{\"error\":\"Please provide all fields\"}", response.Content.ReadAsStringAsync().Result);

        }
    }
}
