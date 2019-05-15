using ASP.NET_Core_Webapp.DTO;
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
    public class PostReviewTest
    {
        private readonly TestContext testContext;

        public PostReviewTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task PostReview_Should_Return_Ok()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/review");
            request.Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostReview_NoBodyContent_ShouldReturn404()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/review");
            request.Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostReview_AllFielsdFilled_ShouldReturn201()
        {
            ReviewDTO review = new ReviewDTO() { Message = "testreview", Status = true, PitchId = 4 };

            var request = new HttpRequestMessage(HttpMethod.Post, "/review");
            request.Content = (new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json"));

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
