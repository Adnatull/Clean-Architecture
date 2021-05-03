using System.Reflection;
using Core.Application.Contracts.Interfaces;
using Core.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Container
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
