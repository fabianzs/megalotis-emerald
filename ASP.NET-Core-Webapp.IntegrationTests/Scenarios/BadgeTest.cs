using ASP.NET_Core_Webapp.Controllers;
using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.IntegrationTests.Fixtures;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Scenarios
{
    [Collection("BaseCollection")]
    public class BadgeTest
    {
        private readonly TestContext testContext;

        public BadgeTest(TestContext testContext)
        {
            this.testContext = testContext;
        }

        [Fact]
        public async Task MyBadges_Should_Return_ReturnOk()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/mybadges");
            HttpResponseMessage response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task MyBadges_Should_Return_Badge_In_Proper_Format()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/mybadges");
            HttpResponseMessage response = await testContext.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal("{\"badges\":[{\"name\":\"English speaker\",\"level\":2},{\"name\":\"Java developer\",\"level\":3},{\"name\":\"Stress management\",\"level\":1},{\"name\":\"Endpoint creater\",\"level\":2},{\"name\":\"Endpoint creater\",\"level\":1},{\"name\":\"Endpoint creater\",\"level\":3},{\"name\":\"Method writer\",\"level\":1},{\"name\":\"Test creator\",\"level\":1}]}", response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task MyBadges_ContentTypeJson_Success()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/mybadges");
            HttpResponseMessage response = await testContext.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task MyBadgesMock_Should_Return_ReturnOk()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/mybadgesmock");
            HttpResponseMessage response = await testContext.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task MyBadgesMock_Should_Return_Badge_In_Proper_Format()
        {
            var response = await testContext.Client.GetAsync("/mybadgesmock");
            response.EnsureSuccessStatusCode();
            Assert.Equal("{\"badges\":[{\"badgeId\":0,\"version\":null,\"name\":\"test\",\"tag\":null,\"levels\":[]}]}",
                response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task MyBadgesMock_ContentTypeJson_Success()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/mybadgesmock");
            var response = await testContext.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
