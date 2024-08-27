namespace TABP.Shared.Exceptions;

public class RoomsNotAvailableException : ConflictException
{
    public RoomsNotAvailableException(IEnumerable<string> unavailableRooms)
        : base(CreateMessage(unavailableRooms))
    {
    }

    private static string CreateMessage(IEnumerable<string> unavailableRooms)
    {
        return "The following rooms are not available:\n" + string.Join("\n", unavailableRooms);
    }
}