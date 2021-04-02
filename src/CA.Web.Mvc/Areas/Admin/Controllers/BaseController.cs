using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            var actionName = actionDescriptor.RouteValues["action"];
            var controllerName = actionDescriptor.RouteValues["controller"];
            ViewData["action"] = actionName;
            ViewData["controller"] = controllerName;
            var runtimeVersion = typeof(Startup)
                .GetTypeInfo()
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            ViewData["mvcVersion"] = runtimeVersion;
        }
    }
}
