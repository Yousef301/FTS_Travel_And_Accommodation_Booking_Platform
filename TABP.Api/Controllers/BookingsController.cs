using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Bookings.CancelBooking;
using TABP.Application.Commands.Bookings.CheckoutBooking;
using TABP.Application.Commands.Bookings.CreateBooking;
using TABP.Application.Queries.Bookings.GetBookingById;
using TABP.Application.Queries.Bookings.GetBookings;
using TABP.Shared.Enums;
using TABP.Web.DTOs.Bookings;
using TABP.Web.Helpers.Interfaces;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/user/bookings")]
[Authorize(Roles = nameof(Role.Customer))]
public class BookingsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public BookingsController(IMapper mapper,
        IMediator mediator,
        IUserContext userContext)
    {
        _mapper = mapper;
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <summary>
    /// Retrieves a list of bookings for the currently authenticated user.
    /// </summary>
    /// <returns>A list of bookings for the user.</returns>
    /// <response code="200">Returns a list of bookings for the authenticated user.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="500">If an internal server error occurs while retrieving bookings.</response>
    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await _mediator.Send(new GetBookingsQuery
        {
            UserId = _userContext.Id
        });

        return Ok(bookings);
    }

    /// <summary>
    /// Retrieves a specific booking by its ID for the currently authenticated user.
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking to be retrieved.</param>
    /// <returns>The details of the requested booking.</returns>
    /// <response code="200">Returns the details of the requested booking.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to access the booking.</response>
    /// <response code="404">The booking with the specified ID was not found.</response>
    /// <response code="500">If an internal server error occurs while retrieving the booking.</response>
    [HttpGet("{bookingId:guid}")]
    public async Task<IActionResult> GetBooking(Guid bookingId)
    {
        var booking = await _mediator.Send(new GetBookingByIdQuery
        {
            BookingId = bookingId,
            UserId = _userContext.Id
        });

        return Ok(booking);
    }

    /// <summary>
    /// Cancels a specific booking by its ID for the currently authenticated user.
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking to be canceled.</param>
    /// <response code="200">The booking was successfully canceled.</response>
    /// <response code="204">The booking was successfully canceled and no additional content is returned.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to cancel the booking.</response>
    /// <response code="404">The booking with the specified ID was not found.</response>
    /// <response code="409">The booking is already canceled or cannot be canceled.</response>
    /// <response code="500">If an internal server error occurs while canceling the booking.</response>
    [HttpPatch("{bookingId:guid}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid bookingId)
    {
        var canceledBooking = await _mediator.Send(new CancelBookingCommand
        {
            UserId = _userContext.Id,
            BookingId = bookingId
        });

        return Ok(canceledBooking);
    }

    /// <summary>
    /// Creates a new booking for the currently authenticated user.
    /// </summary>
    /// <param name="request">The details required to create a new booking.</param>
    /// <returns>The created booking id.</returns>
    /// <response code="201">The booking was successfully created.</response>
    /// <response code="400">If the request contains invalid data or is missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to create a booking.</response>
    /// <response code="500">If an internal server error occurs while creating the booking.</response>
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto request)
    {
        var command = _mapper.Map<CreateBookingCommand>(request);
        command.UserId = _userContext.Id;

        var createdBookingId = await _mediator.Send(command);

        var resourceUri = Url.Action("GetBooking", new { id = createdBookingId });

        return Created(resourceUri, new { id = createdBookingId });
    }

    /// <summary>
    /// Initiates the checkout process for a specific booking by its ID for the currently authenticated user.
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking to check out.</param>
    /// <returns>The URL for completing the checkout process.</returns>
    /// <response code="200">Returns the URL for completing the checkout process.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to perform the checkout.</response>
    /// <response code="404">The booking with the specified ID was not found.</response>
    /// <response code="409">The booking cannot be checked out due to a conflict (booking already processed).</response>
    /// <response code="500">If an internal server error occurs while processing the checkout.</response>
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