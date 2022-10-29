using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Core.Application.Contracts.HandlerExchanges.Post.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PostController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAllPostQueryResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GetAllPostQueryResponse>>> GetPosts()
        {
            var products = await Mediator.Send(new GetAllPostQuery());
            return Ok(products);
        }
    }
}
