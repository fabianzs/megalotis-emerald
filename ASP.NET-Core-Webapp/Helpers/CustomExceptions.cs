using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Helpers.Exceptions
{
    public class PitchIsNullException : Exception
    {
    }

    public class ReviewIsNullException : Exception
    {
    }

    public class InvalidPitchException : Exception
    {
    }

    public class OtherUsersReviewException : Exception
    {
    }

    public class NotAllowedToReviewException : Exception
    {
    }

}
