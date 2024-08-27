namespace TABP.Shared.Exceptions;

public class NoBookingForHotelException : NotFoundException
{
    public NoBookingForHotelException(string message) : base(message)
    {
    }
}