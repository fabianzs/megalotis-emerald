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
    [Collection("BaseCollection")]
    public class PostBadgesEndpointTest
    {
        private readonly TestContext testContext;

        public PostBadgesEndpointTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task PostBadges_NoBodyContent_ShouldReturn403()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/badges")
            {
                Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json")
            };

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task PostBadges_AllFielsdFilled_ShouldReturn201()
        {
            BadgeDTO badge = new BadgeDTO()
            {
                Name = "test",
                Tag = "test",
                Version = "test",
                Levels =
                {
                    new BadgeLevelDTO()
                    {
                        Description = "test",
                        Holders = { "Osztertág Szabolcs", "Zsófia Eszter Fábián" }
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/badges")
            {
                Content = (new StringContent(JsonConvert.SerializeObject(badge), Encoding.UTF8, "application/json"))
            };

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostBadges_NotAllFieldFilledLevelsIsMissing_ShouldReturn401WithBody()
        {
            BadgeDTO badge = new BadgeDTO()
            {
                Name = "test",
                Tag = "test",
                Version = "test",
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/badges")
            {
                Content = new StringContent(JsonConvert.SerializeObject(badge), Encoding.UTF8, "application/json")
            };

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal("{\"Error\":\"Please provide all fields.\"}", response.Content.ReadAsStringAsync().Result);

        }
    }
}