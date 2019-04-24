using ASP.NET_Core_Webapp.Controllers;
using ASP.NET_Core_Webapp.Entities;
using ASP.NET_Core_Webapp.Services;
using System.Collections.Generic;
using Xunit;

namespace ASP.NET_Core_Webapp.UnitTests
{
    public class HelloServiceTests
    {
        private IHelloService helloService;
        private BadgeController controller;


        public HelloServiceTests()
        {
            this.helloService = new HelloService();
            this.controller = new BadgeController();
        }

        [Fact]
        public void HelloWorldReturnIsValid()
        {
            Assert.Equal("Hello World!", helloService.HelloWorld());
        }

        [Fact]
        public void HelloWorldReturnIsInvalid()
        {
            Assert.NotEqual("Yo Mate!", helloService.HelloWorld());
        }

        [Fact]
        public void Badgesendpoint()
        {
            List<Badge> badges = new List<Badge>();
            List<Badge> badgess = new List<Badge>();

            badges.Add(new Badge("test",2));

            Assert.NotEqual(badges, controller.MyBadges());
        }
    }
}

