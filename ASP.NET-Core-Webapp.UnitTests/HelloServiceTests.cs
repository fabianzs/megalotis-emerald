using ASP.NET_Core_Webapp.Services;
using Xunit;

namespace ASP.NET_Core_Webapp.UnitTests
{
    public class HelloServiceTests
    {
        private IHelloService helloService;

        public HelloServiceTests()
        {
            this.helloService = new HelloService();
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
    }
}
