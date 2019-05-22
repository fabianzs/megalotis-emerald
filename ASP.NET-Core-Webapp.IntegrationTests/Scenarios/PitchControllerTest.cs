using ASP.NET_Core_Webapp.DTO;
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
    public class PitchControllerTest
    {
        private readonly TestContext testContext;

        public PitchControllerTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        readonly List<Review> Holders = new List<Review>{

            new Review( "Good", true),
            new Review( "Good", true),
            new Review( "Good", true),
            };

        [Fact]
        public async Task CreateNewPitch_Should_Return201()
        {

            PitchDTO pitch = new PitchDTO() { BadgeName ="Hamburger destroyer", OldLVL ="2", PitchedLVL= 3, PitchMessage ="test" };
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));
            
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateNewPitchIsNullTest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateNewPitchPitchAlreadyExist()
        {
            Pitch pitch = new Pitch( new Badge(), 2, 3, "Hello World! My English is bloody gorgeous.", Holders);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PitchUpdateTest()
        {
            Pitch pitch = new Pitch( new Badge(), 2, 3, "Hello World! My English is bloody gorgeous.", Holders);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitch");
            var response = await testContext.Client.PostAsync("/api/pitch", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PitchExistsTest()
        {
            Pitch pitch = new Pitch( new Badge(), 2, 3, "Hello World! My English is bloody gorgeous.", Holders);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitch");
            var response = await testContext.Client.PostAsync("/api/pitch", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
