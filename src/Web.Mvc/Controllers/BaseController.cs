using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Mvc.Controllers
{
    /// <summary>
    /// Base Controller
    /// </summary>
    public abstract class  BaseController : Controller
    {
        private IMediator _mediator;

        /// <summary>
        /// MediaTr Instance
        /// </summary>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
