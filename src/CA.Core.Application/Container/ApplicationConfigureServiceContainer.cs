using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Core.Application.Container
{
    public static class ApplicationConfigureServiceContainer
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
