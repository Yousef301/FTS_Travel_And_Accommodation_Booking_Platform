﻿namespace TABP.Domain.Exceptions;

public class BookingStatusException : BadRequestException
{
    public BookingStatusException(string? message = null) : base(message ?? "Booking is already confirmed or canceled.")
    {
    }
}