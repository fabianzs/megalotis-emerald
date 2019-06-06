using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.DTO
{
    public class MyPitchesDTO
    {
        public DateTime TimeStamp { get; set; }
        public string Username { get; set; }
        public string BadgeName { get; set; }
        public int OldLevel { get; set; }
        public int PitchedLevel { get; set; }
        public string PitchMessage { get; set; }
        public List<MyReviewsDTO> Holders { get; set; }
    }

    public class MyReviewsDTO
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public bool? PitchStatus { get; set; }
    }
}
