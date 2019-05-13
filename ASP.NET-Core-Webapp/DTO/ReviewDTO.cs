using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.DTO
{
    public class ReviewDTO
    {
        public string Message { get; set; }
        public bool? Status { get; set; }
        public int? PitchId { get; set; }
    }
}
