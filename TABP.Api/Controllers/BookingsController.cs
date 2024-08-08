using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Bookings.CancelBooking;
using TABP.Application.Commands.Bookings.CheckoutBooking;
using TABP.Application.Commands.Bookings.CreateBooking;
using TABP.Application.Queries.Bookings.GetBookingById;
using TABP.Application.Queries.Bookings.GetBookings;
using TABP.Domain.Enums;
using TABP.Web.DTOs;
using TABP.Web.DTOs.Bookings;
using TABP.Web.Services.Interfaces;

namespace TABP.Web.Controllers;

[ApiController]
[Route("api/user/bookings")]
[Authorize(Roles = nameof(Role.Customer))]
public class BookingsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public BookingsController(IMapper mapper, IMediator mediator, IUserContext userContext)
    {
        _mapper = mapper;
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await _mediator.Send(new GetBookingsQuery
        {
            UserId = _userContext.Id
        });

        return Ok(bookings);
    }

    [HttpGet("{bookingId:guid}")]
    public async Task<IActionResult> GetBooking(Guid bookingId)
    {
        var booking = await _mediator.Send(new GetBookingByIdQuery
        {
            BookingId = bookingId
        });

        return Ok(booking);
    }

    [HttpPatch("{bookingId:guid}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid bookingId)
    {
        await _mediator.Send(new CancelBookingCommand
        {
            BookingId = bookingId
        });

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto request)
    {
        var command = _mapper.Map<CreateBookingCommand>(request);
        command.UserId = _userContext.Id;

        await _mediator.Send(command);

        return Created();
    }

    [HttpPost("{bookingId:guid}/checkout")]
    public async Task<IActionResult> CheckoutBooking(Guid bookingId)
    {
        var checkoutUrl = await _mediator.Send(new CheckoutBookingCommand
        {
            UserEmail = _userContext.Email,
            UserId = _userContext.Id,
            BookingId = bookingId,
        });

        return Ok(new { Url = checkoutUrl });
    }
}