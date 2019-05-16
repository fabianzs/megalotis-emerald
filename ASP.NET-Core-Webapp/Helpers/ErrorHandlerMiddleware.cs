using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASP.NET_Core_Webapp.Helpers.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
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
            if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorMessage.Error = "You are not allowed to give a review.";
            }
            else if (exception is PitchIsNullException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorMessage.Error = "The provided pitch does not exist.";
            }
            else if(exception is NotAllowedToReviewException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorMessage.Error = "You are not allowed to give a review, because you either lack the necessary badge, or you are trying to review your own pitch.";
            }
            else if (exception is ReviewIsNullException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorMessage.Error = "The provided review does not exist.";
            }
            else if (exception is InvalidPitchException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorMessage.Error = "The review cannot be updated, it belongs to another pitch.";
            }
            else if (exception is OtherUsersReviewException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorMessage.Error = "The review cannot be updated, it belongs to another user.";
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                errorMessage.Error = "Ther error is not specified, You have not reviewed this pitch yet.";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
        }
    }
}



