using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


namespace Framework.Core.Globalization
{
    public class GlobalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Do something with context near the beginning of request processing.
            CultureHelper.InitializeCultureFromCookie(context);

            await _next.Invoke(context);
            // Clean up.
        }
    }

    public static class GlobalizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalizationMiddleware>();
        }
    }
}