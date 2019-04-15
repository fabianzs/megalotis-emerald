using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Models
{
    public class User
    {
        public Pitch[] myPitches;
        public Pitch[] pitchesToReview;

        public User(Pitch[] myPitches, Pitch[] pitchesToReview)
        {
            this.myPitches = myPitches;
            this.pitchesToReview = pitchesToReview;
        }
    }
}
