using CA.Web.Framework.Middleware;
using Microsoft.AspNetCore.Builder;

namespace CA.Web.Framework.Extensions
{
    public static class ConfigureContainer
    {
        public static void UseApiErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiErrorHandlerMiddleware>();
        }
    }
}
