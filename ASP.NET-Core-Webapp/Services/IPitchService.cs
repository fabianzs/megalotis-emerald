using ASP.NET_Core_Webapp.DTO;
using ASP.NET_Core_Webapp.Entities;
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
        bool PutPitch(Pitch pitch, HttpRequest request);
    }
}
