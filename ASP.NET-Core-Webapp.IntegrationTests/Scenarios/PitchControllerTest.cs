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
        public async Task CreateNewPitchSuccessTest()
        {
            Pitch pitch = new Pitch(new User(),new Badge(), 2,3, "Hello World! My English is bloody gorgeous.", Holders);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));
           
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateNewPitchIsNullTest()
        {     
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateNewPitchPitchAlreadyExist()
        {
            Pitch pitch = new Pitch(new User(), new Badge(), 2, 3, "Hello World! My English is bloody gorgeous.", Holders);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

    }
}
