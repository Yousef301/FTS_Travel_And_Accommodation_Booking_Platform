namespace TABP.Domain.Exceptions;

public class BookingStatusException : Exception
{
    public BookingStatusException(string? message = null) : base(message ?? "Booking is already confirmed or canceled.")
    {
    }
}