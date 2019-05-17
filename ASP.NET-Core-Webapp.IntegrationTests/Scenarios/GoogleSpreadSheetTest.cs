using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    [Collection("BaseCollection")]
    public class GoogleSpreadSheetTest
    {
        private readonly TestContext testContext;
        

        public GoogleSpreadSheetTest(TestContext testContext)
        {
            this.testContext = testContext;
            
        }

        [Fact]
        public async Task SpreadSheet_Should_Return_OK()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "spreadsheet");
            var response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SpreadSheet_Should_Fill_Up_Db()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "spreadsheet");
            var response = await testContext.Client.SendAsync(request);
            int ProperBadgeCounter = testContext.context.Badges.Where(badge => badge.Name.Equals("test")).Count();
            Assert.Equal(1, ProperBadgeCounter);
        }
    }
}
