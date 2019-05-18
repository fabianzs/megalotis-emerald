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
    public class PutReviewTest
    {
        private readonly TestContext testContext;

        public PutReviewTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task PutReview_NoBodyContent_ShouldReturn404()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "/review/12");
            request.Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutReview_AllFielsdFilledUserHasGivenReview_ShouldReturn200()
        {
            ReviewDTO review = new ReviewDTO() { Message = "testreview", Status = true, PitchId = 3 };
            var request = new HttpRequestMessage(HttpMethod.Put, "/review/9");
            request.Content = (new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json"));

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PutReview_AllFielsdFilledUserHasNotGivenReview_ShouldReturn400()
        {
            ReviewDTO review = new ReviewDTO() { Message = "testreview", Status = true, PitchId = 5 };

            var request = new HttpRequestMessage(HttpMethod.Put, "/review/9");
            request.Content = (new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json"));

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutReview_AllFielsdFilledReviewDoesNotMatchPitch_ShouldReturn401()
        {
            ReviewDTO review = new ReviewDTO() { Message = "testreview", Status = true, PitchId = 5 };

            var request = new HttpRequestMessage(HttpMethod.Put, "/review/15");
            request.Content = new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json");

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
