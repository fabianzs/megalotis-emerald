using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class PitchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}