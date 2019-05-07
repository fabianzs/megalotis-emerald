using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    
    [ApiController]
    public class SpreadSheetController : ControllerBase
    {
        GoogleSheetService GoogleSheetService;

        public SpreadSheetController(GoogleSheetService googleSheetService)
        {
            GoogleSheetService = googleSheetService;
        }

        [HttpGet("spreadsheet")]
        public IActionResult test(FromBodyAttribute link)
        {
            var teszt = GoogleSheetService.AccesTokenPublic;
            return Redirect("https://sheets.googleapis.com/v4/spreadsheets/1oW7QEQVR_-aX3UYgN2n03UUB2GDAFqhE_DPRgPuukAI/values/Munkalap1");
        }
    }
}