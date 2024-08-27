namespace TABP.Shared.Exceptions;

public class BookingOverlapException : ConflictException
{
    public BookingOverlapException()
        : base("Booking is overlaps with an existing booking.")
    {
    }
}