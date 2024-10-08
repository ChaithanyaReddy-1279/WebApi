using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyWebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, 
                RequestDelegate next)
        {
            this.next = next;
            this.logger = logger;
            
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (System.Exception ex)
            {
                var errorId = Guid.NewGuid();
                //Log this Exception and return custom response back
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "An error occurred while processing your request."
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
        
    }
}