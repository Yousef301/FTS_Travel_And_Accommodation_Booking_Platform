namespace TABP.Domain.Exceptions;

public class BookingOverlapException : ConflictException
{
    public BookingOverlapException()
        : base("Booking is overlapping with an existing booking.")
    {
    }
}