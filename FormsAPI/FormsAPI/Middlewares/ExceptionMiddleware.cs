using Elastic.Clients.Elasticsearch.Inference;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnixLabs.Core.Linq;
using System.Net.Http;

namespace FormsAPI.Middlewares
{
    public class ExceptionMiddleware : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            if (exception is ArgumentNullException) await HandleArgumentNullException(httpContext,exception);
            else if (exception is NullReferenceException) await HandleNullReferenceException(httpContext, exception);
            else await HandleException(httpContext, exception);
            return true;
        }

        private async Task HandleException(HttpContext httpContext, Exception exception)
        {
            await httpContext.Response.WriteAsJsonAsync($"An Error occured {exception.InnerException}");
        }

        private async Task HandleArgumentNullException(HttpContext httpContext,Exception exception)
        {
            await httpContext.Response.WriteAsJsonAsync($"object not found: {exception.Message}");
        }
        private async Task HandleNullReferenceException(HttpContext httpContext, Exception exception)
        {
            await httpContext.Response.WriteAsJsonAsync($"null reference: {exception.Message}");
        }
    }
}
