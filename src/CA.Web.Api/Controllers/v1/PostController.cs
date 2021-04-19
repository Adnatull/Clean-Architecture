using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CA.Core.Application.Contracts.Handlers.Post.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PostController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAllPostQueryViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GetAllPostQueryViewModel>>> GetProducts()
        {
            var products = await Mediator.Send(new GetAllPostQuery());
            return Ok(products);
        }
    }
}
