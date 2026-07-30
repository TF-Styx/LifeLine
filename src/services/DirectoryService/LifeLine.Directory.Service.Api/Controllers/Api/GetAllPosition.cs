using LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAllPosition;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/positions/get-all")]
    [Authorize]
    public class GetAllPosition(IMediator mediator) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPositionAsync(CancellationToken cancellationToken = default)
            => Ok(await mediator.Send(new GetAllPositionQuery(), cancellationToken));
    }
}
