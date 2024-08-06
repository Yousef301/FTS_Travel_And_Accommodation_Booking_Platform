﻿using MediatR;

namespace TABP.Application.Commands.Bookings.CreateBooking;

public class CreateBookingCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public IEnumerable<Guid> RoomIds { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public string PaymentMethod { get; set; }
}