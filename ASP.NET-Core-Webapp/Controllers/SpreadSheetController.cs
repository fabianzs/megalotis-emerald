using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class SpreadsheetController : Controller
    {
        [HttpGet("spreadsheet")]
        public Object SpreadSheet()
        {
           // Console.WriteLine(ReadAccesTokenFromFile());
            return ReadAccesTokenFromFile();
        }

        private string ReadAccesTokenFromFile()
        {
            return System.IO.File.ReadAllText("AccesToken.txt");
        }
    }
}