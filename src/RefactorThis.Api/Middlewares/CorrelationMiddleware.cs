using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace RefactorThis.Api.Middlewares
{
    public class CorrelationMiddleware
    {
        private const string _correlationHeader = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(_correlationHeader))
            {
                var correlationId = context.Request.Headers[_correlationHeader].FirstOrDefault() ?? Guid.NewGuid().ToString();

                context
                    .Response
                    .Headers
                    .Add(_correlationHeader, correlationId);

                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    return _next(context);
                }
            }

            return _next(context);
        }
    }
}