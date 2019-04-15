using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
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
    public class PitchTest
    {
        private readonly TestContext testContext;

        public PitchTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task Pitch_GetPitches_Return200()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/pitches");
            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
