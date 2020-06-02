using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using RefactorThis.Api.Models;
using RefactorThis.Core.Exceptions;

namespace RefactorThis.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case EntityNotFoundException _:
                    context.Result = new NotFoundObjectResult(new ErrorResponse { Title = context.Exception.Message });
                    break;

                default:
                    _logger.LogError(context.Exception, "Unhandled exception occured while fulfilling Http request");
                    context.Result = new ObjectResult(new ErrorResponse("An unexpected error was encountered"))
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                    break;
            }
        }
    }
}