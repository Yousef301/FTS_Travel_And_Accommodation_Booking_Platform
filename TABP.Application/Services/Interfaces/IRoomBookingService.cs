namespace TABP.Application.Services.Interfaces;

public interface IRoomBookingService
{
    Task<bool> IsRoomAvailable(Guid roomId, DateOnly startDate, DateOnly endDate);
}