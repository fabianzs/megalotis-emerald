using System;

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

    public class NoMessageBodyException : Exception
    {
    }

    public class MissingFieldsException : Exception
    {
    }

    public class UserNotFoundException : Exception
    {
    }
}
