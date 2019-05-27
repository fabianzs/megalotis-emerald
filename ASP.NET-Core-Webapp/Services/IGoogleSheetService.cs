using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public interface IGoogleSheetService
    {
       Task FillUpDataBaseFromSpreadSheet();
       Task<string> ReturnBadgesSpreadSheetContent();
       HttpRequestMessage ReturnSpreadSheetRequest();
       string MakeGoogleSheetApiURL();
    }
}
