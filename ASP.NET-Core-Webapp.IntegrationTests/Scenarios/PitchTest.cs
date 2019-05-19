using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public PitchTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task Pitch_GetPitches_Return202()
        {
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
            Debugger.Launch();
            var request = new HttpRequestMessage(HttpMethod.Post, "/pitches");
            request.Content = new StringContent(await 
                new StreamReader(@"C:\Users\laszl\Documents\Programozás\greenfox\megem_project\megalotis-emerald\ASP.NET-Core-Webapp.IntegrationTests\PitchPostTest.json").ReadToEndAsync(), 
                Encoding.UTF8,
                "application/json"
                );

            var response = await testContext.Client.SendAsync(request);

            Assert.Equal<HttpStatusCode>(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
