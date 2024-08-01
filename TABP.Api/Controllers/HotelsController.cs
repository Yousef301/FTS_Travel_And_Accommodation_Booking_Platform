using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Web.Enums;

namespace TABP.Web.Controllers;

[ApiController]
[Route("api/hotels")]
[Authorize(Roles = nameof(Role.Admin))]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public HotelsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetHotels(CancellationToken cancellationToken)
    // {
    //     var query = new GetHotelsQuery();
    //     var hotels = await _mediator.Send(query, cancellationToken);
    //     return Ok();
    // }
}