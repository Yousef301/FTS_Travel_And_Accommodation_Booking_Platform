using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Hotels.CreateHotel;
using TABP.Application.Commands.Hotels.DeleteHotel;
using TABP.Application.Commands.Hotels.UpdateHotel;
using TABP.Application.Queries.Hotels.GetHotelsForAdmin;
using TABP.Application.Queries.Hotels.GetHotelsForUser;
using TABP.Application.Queries.Hotels.GetHotelsWithFeaturedDeals;
using TABP.Domain.Enums;
using TABP.Domain.Extensions;
using TABP.Web.DTOs;
using TABP.Web.DTOs.Hotels;
using TABP.Web.Extensions;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/hotels")]
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
    public async Task<IActionResult> GetHotelsForUsers([FromQuery] HotelsSearchDto searchDto)
    {
        var query = _mapper.Map<GetHotelsForUserQuery>(searchDto);

        var hotels = await _mediator.Send(query);

        var metadata = hotels.ToMetadata();

        Response.AddPaginationMetadata(metadata, Request);

        return Ok(hotels.Items);
    }

    [HttpGet("featured-deals")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeaturedDeals([FromQuery] int count = 5)
    {
        var hotels = await _mediator.Send(
            new GetHotelsWithFeaturedDealsQuery
            {
                Count = count
            });

        return Ok(hotels);
    }

    [HttpGet]
    public async Task<IActionResult> GetHotelsForAdmin([FromQuery] FilterParameters filterParameters)
    {
        var query = _mapper.Map<GetHotelsForAdminQuery>(filterParameters);

        var hotels = await _mediator.Send(query);

        var metadata = hotels.ToMetadata();

        Response.AddPaginationMetadata(metadata, Request);

        return Ok(hotels.Items);
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