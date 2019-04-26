
//using ASP.NET_Core_Webapp.Entities;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;

//namespace ASP.NET_Core_Webapp.Controllers
//{
//    [Route("api")]
//    [ApiController]
//    public class PitchController : Controller
//    {
//         readonly List<Review> Holders = new List<Review>{
//            new Review("Szabi", "Good", true),
//            new Review("Zsófi", "Good", true),
//            new Review("Laci", "Good", true),
//            };

//        List<Pitch> pitches = new List<Pitch>();

//        public PitchController()
//        {
//            pitches.Add(new Pitch("Boti", "C#", 1, 2, "I have been improving", Holders));
//            pitches.Add(new Pitch("Boti", "C++ pro", 1, 2, "I have been improving", Holders));
//            pitches.Add(new Pitch("Boti", "Java pro", 1, 2, "I have been improving", Holders));
//            pitches.Add(new Pitch("Boti", "C# pro", 1, 2, "I have been improving", Holders));
//        }


//        [HttpGet("pitches")]
//        public Object GetPitch()
//        {


//            Pitch pitch = new Pitch("Boti", "C#", 1, 2, "I have been improving", Holders);
//            Pitch pitch2 = new Pitch("Boti", "Java", 1, 2, "I stopped learning for a while", Holders);
//            Pitch pitch3 = new Pitch("Boti", "Macro", 1, 2, "I have been improving", Holders);

//            User user = new User(new List<Pitch> { pitch, pitch2, pitch3 }, new List<Pitch> { pitch, pitch3 });

//            if (user != null)
//            {
//                return user;
//            }

//            return StatusCode(401, new { error = "Unauthorizied" });

//        }
//        [HttpPost("pitches")]
//        public IActionResult CreateNewPitch(Pitch newPitch)
//        {
//            if (!pitches.Exists(e => e.BadgeName.Equals(newPitch.BadgeName)) && !newPitch.Equals(null))
//            {
//                pitches.Add(newPitch);
//                return Created("", new { message = "Success" });
//            }
//            else
//            {
//                return Unauthorized(new { error = "Unauthorizied" });
//            }
//        }
//    }
//}