namespace TABP.Web.DTOs.Rooms;

public class GetAvailableRoomsDto
{
    public DateOnly CheckInDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly CheckOutDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
}