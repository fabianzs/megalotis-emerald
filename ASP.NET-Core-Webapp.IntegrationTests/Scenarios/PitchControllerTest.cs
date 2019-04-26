//using ASP.NET_Core_Webapp.Entities;
//using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
//using Newtonsoft.Json;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using System.Collections.Generic;

//namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
//{
//    [Collection("BaseCollection")]
//    public class PitchControllerTest
//    {
//        private readonly TestContext testContext;

//        public PitchControllerTest(TestContext testContext)
//        {
//            this.testContext = testContext;
//        }

//        readonly List<Review> Holders = new List<Review>{
//            new Review("Szabi", "Good", true),
//            new Review("Zsófi", "Good", true),
//            new Review("Laci", "Good", true),
//            };

//        [Fact]
//        public async Task CreateNewPitchSuccessTest()
//        {
//            Pitch pitch = new Pitch("Boti", "English speaker", 1, 2, "I have been improving", Holders);

//            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
//            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));
           
//            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//        }

//        [Fact]
//        public async Task CreateNewPitchIsNullTest()
//        {     
//            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
//            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json"));

//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//        }

//        [Fact]
//        public async Task CreateNewPitchPitchAlreadyExist()
//        {
//            Pitch pitch = new Pitch("Boti", "C#", 1, 2, "I have been improving", Holders);

//            var request = new HttpRequestMessage(HttpMethod.Post, "/api/pitches");
//            var response = await testContext.Client.PostAsync("/api/pitches", new StringContent(JsonConvert.SerializeObject(pitch), Encoding.UTF8, "application/json"));

//            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//        }

//    }
//}
