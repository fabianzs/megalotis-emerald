using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using ASP.NET_Core_Webapp.Models;
using Newtonsoft.Json;
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

        [Fact]
        public async Task CreateNewPitchSuccessTest()
        {
            Pitch pitch = new Pitch() { BadgeName = "English speaker", OldLevel = 2, PitchedLevel = 3, PitchMessage = "Hello World! My English is bloody gorgeous.", Holders = { "balazs.jozsef", "benedek.vamosi", "balazs.barna" } };

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
            Pitch pitch = new Pitch() { BadgeName = "C# pro", OldLevel = 2, PitchedLevel = 3, PitchMessage = "Hello World! My English is bloody gorgeous.", Holders = { "balazs.jozsef", "benedek.vamosi", "balazs.barna" } };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

    }
}
