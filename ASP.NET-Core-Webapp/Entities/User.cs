using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Entities
{
    public class User
    { 
        public ICollection<Pitch> myPitches;
        public ICollection<Pitch> pitchesToReview;

        public User(ICollection<Pitch> myPitches, ICollection<Pitch> pitchesToReview)
        {
            this.myPitches = myPitches;
            this.pitchesToReview = pitchesToReview;
        }

        public User()
        {

        }
    }
}
