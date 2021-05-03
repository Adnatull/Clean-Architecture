using Microsoft.AspNetCore.Builder;
using Web.Framework.Middleware;

namespace Web.Framework.Extensions
{
    public static class ConfigureContainer
    {
        public static void UseApiErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiErrorHandlerMiddleware>();
        }
    }
}
