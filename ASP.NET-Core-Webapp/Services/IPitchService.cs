using ASP.NET_Core_Webapp.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public interface IPitchService
    {
        bool PostPitch(HttpRequest request, PitchDTO pitchDTO);
    }
}
