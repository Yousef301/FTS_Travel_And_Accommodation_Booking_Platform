namespace TABP.Domain.Exceptions;

public class NoBookingForHotelException : NotFoundException
{
    public NoBookingForHotelException(string message) : base(message)
    {
    }
}