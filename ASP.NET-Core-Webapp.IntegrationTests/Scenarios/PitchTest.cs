using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    [Collection("BaseCollection")]
    public class PitchTest
    {
        private readonly TestContext testContext;

        public PitchTest(TestContext tc)
        {
            this.testContext = tc;
        }

        [Fact]
        public async Task Pitch_GetPitches_Return202()
        {
<<<<<<< HEAD
=======
            //Debugger.Launch();
>>>>>>> dev
            var request = new HttpRequestMessage(HttpMethod.Get, "/pitches");
            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Theory]
        [InlineData("/pitches")]
        public async Task Pitch_GetPitchesInLineData_Return202(string url)
        {
            var request = url;
            var response = await testContext.Client.GetAsync(request);
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Pitch_ContentTypeJson_Success()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/pitches");
            var response = await testContext.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Pitch_UserNotNull_NotEqual()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/pitches");
            var response = await testContext.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.NotEqual("{\"myPitches\":null,\"pitchesToReview\":null}", response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task Pitch_PostNewPitch_ReturnOK()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/pitches");
            using (StreamReader sr = new StreamReader("../../../PitchPostTest.json"))
            {
                string contentLoad = await sr.ReadToEndAsync();
                request.Content = new StringContent(contentLoad,
                    Encoding.UTF8,
                    "application/json"
                    );
            }

            var response = await testContext.Client.SendAsync(request);

            Assert.Equal<HttpStatusCode>(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
