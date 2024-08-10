namespace TABP.Domain.Exceptions;

public class BookingOverlapException : Exception
{
    public BookingOverlapException()
        : base("Booking is overlapping with an existing booking.")
    {
    }
}