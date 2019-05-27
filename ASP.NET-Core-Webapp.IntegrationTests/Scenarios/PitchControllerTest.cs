using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    [Collection("BaseCollection")]
    public class PitchControllerTest
    {
        private readonly TestContext testContext;

        public PitchControllerTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task CreateNewPitch_Should_Return201()
        {

            PitchDTO pitch = new PitchDTO() { BadgeName = "English speaker", OldLVL =2, PitchedLVL= 3, PitchMessage ="test" };
            var request = new HttpRequestMessage(HttpMethod.Post, "/pitches");
            var response = await testContext.Client.PostAsync("/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));
            
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateNewPitchIsNullTest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/pitches");
            var response = await testContext.Client.PostAsync("/pitches", new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateNewPitchPitchAlreadyExist()
        {
            List<Review> Holders = new List<Review>();
            Pitch pitch = new Pitch(new Badge("kaka"),2,3, "English speaker", Holders);
            testContext.context.Add(pitch);
            testContext.context.SaveChanges();

            var request = new HttpRequestMessage(HttpMethod.Post, "/pitches");
            var response = await testContext.Client.PostAsync("/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PitchUpdateTest()
        {
            List<Review> Holders = new List<Review>();
            Pitch pitch = testContext.context.Pitches.Include(p => p.User).FirstOrDefault(t => t.PitchId == 1);
            //pitch.User = testContext.context.Users.FirstOrDefault(u => u.Name == "balazs.barna");
            

            var request = new HttpRequestMessage(HttpMethod.Put, "/pitch");
            var response = await testContext.Client.PutAsync("/pitch", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PitchExistsTest()
        {
            List<Review> Holders = new List<Review>();
            Pitch pitch = new Pitch(new Badge("kaka"), 2, 3, "English speaker", Holders);
            testContext.context.Add(pitch);
            testContext.context.SaveChanges();

            var request = new HttpRequestMessage(HttpMethod.Put, "/pitch");
            var response = await testContext.Client.PutAsync("/pitch", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
