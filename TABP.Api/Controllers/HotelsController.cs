using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Hotels.CreateHotel;
using TABP.Application.Commands.Hotels.DeleteHotel;
using TABP.Application.Commands.Hotels.UpdateHotel;
using TABP.Application.Queries.Hotels.GetHotels;
using TABP.Application.Queries.Hotels.GetHotelsWithFeaturedDeals;
using TABP.Domain.Enums;
using TABP.Web.DTOs.Hotels;

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

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHotels([FromQuery] HotelsSearchDto searchDto)
    {
        var query = _mapper.Map<GetHotelsQuery>(searchDto);

        var hotels = await _mediator.Send(query);

        return Ok(hotels);
    }

    [HttpGet("featured-deals")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeaturedDeals()
    {
        var hotels = await _mediator.Send(
            new GetHotelsWithFeaturedDealsQuery());

        return Ok(hotels);
    }


    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto request)
    {
        var command = _mapper.Map<CreateHotelCommand>(request);

        var hotel = await _mediator.Send(command);

        return Ok(hotel);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await _mediator.Send(new DeleteHotelCommand { Id = id });

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateHotel(Guid id,
        [FromBody] JsonPatchDocument<UpdateHotelDto> hotelUpdateDto)
    {
        var hotelDocument = _mapper.Map<JsonPatchDocument<HotelUpdate>>(hotelUpdateDto);

        await _mediator.Send(new UpdateHotelCommand
        {
            Id = id,
            HotelDocument = hotelDocument
        });

        return Ok();
    }
}