using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace BuildingBlocks.Exceptions.handlers
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler

    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message: {ExceptionMessage}, Time of occurence {time}",
                exception.Message, DateTime.UtcNow);
            //pattern matching
            (string Detail, string Title, int statusCode) details = exception switch
            {
                InternalServerException =>
                (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                  exception.Message,
                  exception.GetType().Name,
                  httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                  exception.Message,
                  exception.GetType().Name,
                  httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
               (
                 exception.Message,
                 exception.GetType().Name,
                 httpContext.Response.StatusCode = StatusCodes.Status404NotFound
               ),
                _ =>
                (
                  exception.Message,
                  exception.GetType().Name,
                  httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                )
            };


            var problemDetail = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.statusCode
            };
            problemDetail.Extensions.Add("TraceId",httpContext.TraceIdentifier);
            if (exception is ValidationException validationException)
            {
                problemDetail.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetail,cancellationToken:cancellationToken);

            return true;
        }
    }
}

