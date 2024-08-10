namespace TABP.Domain.Exceptions;

public class NoBookingForHotelException : Exception
{
    public NoBookingForHotelException(string message) : base(message)
    {
    }
}