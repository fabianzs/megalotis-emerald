using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Webapp.Controllers
{
    public class SpreadsheetController : Controller
    {
        IGoogleSheetService googleSheetService;

        public SpreadsheetController(IGoogleSheetService googleSheetService)
        {
            this.googleSheetService = googleSheetService;
        }

        [Authorize("Bearer")]
        [HttpGet("spreadsheet")]
        public async Task<IActionResult> SpreadSheet()
        {
            await googleSheetService.FillUpDataBaseFromSpreadSheet();
            return Ok();
        }


    }
}