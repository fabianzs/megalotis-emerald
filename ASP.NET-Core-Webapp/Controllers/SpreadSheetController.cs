using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class SpreadsheetController : Controller
    {
        GoogleSheetService googleSheetService;

        public SpreadsheetController(GoogleSheetService googleSheetService)
        {
            this.googleSheetService = googleSheetService;
        }

        [HttpGet("spreadsheet")]
        public Object SpreadSheet()
        { 
            return googleSheetService.GetBadgesFromStyleSheet();
        }


    }
}