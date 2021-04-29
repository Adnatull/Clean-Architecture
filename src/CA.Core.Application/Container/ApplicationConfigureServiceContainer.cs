using System.Reflection;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Core.Application.Container
{
    public static class ApplicationConfigureServiceContainer
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
        }
    }
}
