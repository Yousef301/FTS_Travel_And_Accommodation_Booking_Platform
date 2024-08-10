namespace TABP.Domain.Exceptions;

public class RoomsNotAvailableException : Exception
{
    public RoomsNotAvailableException(IEnumerable<string> unavailableRooms)
        : base(CreateMessage(unavailableRooms))
    {
    }

    private static string CreateMessage(IEnumerable<string> unavailableRooms)
    {
        return "The following rooms are not available:\n" + string.Join(Environment.NewLine, unavailableRooms);
    }
}