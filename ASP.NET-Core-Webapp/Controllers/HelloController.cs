using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    [Route("api")]
    public class HelloController : Controller
    {
        private readonly IHelloService helloService;

        public HelloController(IHelloService helloService)
        {
            this.helloService = helloService;
        }


    }
}
