using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;

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
        public async Task PostReview_NoBodyContent_ShouldReturn404()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/review")
            {
                Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json")
            };

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostReview_AllFielsdFilledReviewerOwnsBadge_ShouldReturn201()
        {
            ReviewDTO review = new ReviewDTO() { Message = "testreview", Status = true, PitchId = 3 };

            var request = new HttpRequestMessage(HttpMethod.Post, "/review")
            {
                Content = (new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json"))
            };

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostReview_MessageSent_ShouldReturn201()
        {
            User user = new User() { Email = "fabian.zsofia.eszter@gmail.com", Name = "fabian.zsofia" };
            user.Pitches = new List<Pitch> { { new Pitch() { Badge = testContext.context.Badges.Include(b => b.Levels).FirstOrDefault(b => b.Name.Equals("Programming")), BadgeLevel = testContext.context.BadgeLevels.FirstOrDefault(bl => bl.BadgeLevelId == 26) } } };
            user.UserLevels = new List<UserLevel> { new UserLevel() { BadgeLevel = testContext.context.BadgeLevels.FirstOrDefault(bl => bl.BadgeLevelId == 26) } };
            testContext.context.Add(user);
            testContext.context.SaveChanges();

            ReviewDTO review = new ReviewDTO() { Message = "testreview", Status = true, PitchId = 7 };

            var request = new HttpRequestMessage(HttpMethod.Post, "/review")
            {
                Content = (new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json"))
            };

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostReview_AllFielsdFilledSelfReview_ShouldReturn401()
        {
            ReviewDTO review = new ReviewDTO() { Message = "testreview", Status = true, PitchId = 5 };

            var request = new HttpRequestMessage(HttpMethod.Post, "/review")
            {
                Content = (new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json"))
            };

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task PostReview_AllFielsdFilledPithDoesNotExist_ShouldReturn404()
        {
            ReviewDTO review = new ReviewDTO() { Message = "testreview", Status = true, PitchId = 8 };

            var request = new HttpRequestMessage(HttpMethod.Post, "/review")
            {
                Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json")
            };

            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
