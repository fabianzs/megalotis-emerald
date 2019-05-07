using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class SpreadsheetController : Controller
    {
        [HttpGet("spreadsheet")]
        public Object SpreadSheet()
        {
            return ReadAccesTokenFromFile();
        }

        [HttpGet("spreadsheet2")]
        public object SpreadSheet2()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "https://sheets.googleapis.com/v4/spreadsheets/1oW7QEQVR_-aX3UYgN2n03UUB2GDAFqhE_DPRgPuukAI/values/Munkalap1?access_token=" + ReadAccesTokenFromFile());
            HttpResponseMessage response = new HttpClient().SendAsync(req).Result;
            string res = response.Content.ReadAsStringAsync().Result;
            return res;
        }

        private string ReadAccesTokenFromFile()
        {
            return System.IO.File.ReadAllText("AccesToken.txt");
        }
    }
}