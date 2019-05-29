using System;
using System.Net;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Helpers.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace ASP.NET_Core_Webapp.Helpers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            CustomErrorMessage errorMessage = new CustomErrorMessage();
            int statusCode = 0;
            if(exception is PitchIsNullException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorMessage.Error = "The provided pitch does not exist.";
            }
            else if (exception is NotAllowedToReviewException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorMessage.Error = "You are not allowed to give a review, because you either lack the necessary badge, or you are trying to review your own pitch.";
            }
            else if(exception is ReviewIsNullException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorMessage.Error = "The provided review does not exist.";
            }
            else if(exception is InvalidPitchException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorMessage.Error = "The review cannot be updated, it belongs to another pitch.";
            }
            else if(exception is OtherUsersReviewException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorMessage.Error = "The review cannot be updated, it belongs to another user.";
            }
            else if(exception is NoMessageBodyException)
            {
                statusCode = (int)HttpStatusCode.Forbidden;
                errorMessage.Error = "No message body.";
            }
            else if(exception is MissingFieldsException)
            {
                statusCode = (int)HttpStatusCode.Forbidden;
                errorMessage.Error = "Please provide all fields.";
            }
            else if(exception is UserNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorMessage.Error = "Unknown user";
            }
            else if (exception is PitchIsNullException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorMessage.Error = "There is no such pitch";
            }
            else if (exception is OtherUsersPitchException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorMessage.Error = "The pitch cannot be updated, it belongs to another user.";
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                errorMessage.Error = $"{exception}";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
        }
    }
}
