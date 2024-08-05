using TABP.Application.Services.Interfaces;
using TABP.DAL.Enums;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Services.Implementations;

public class RoomBookingService : IRoomBookingService
{
    private readonly IBookingDetailRepository _bookingDetailRepository;

    public RoomBookingService(IBookingDetailRepository bookingDetailRepository)
    {
        _bookingDetailRepository = bookingDetailRepository;
    }

    public async Task<bool> IsRoomAvailable(Guid roomId, DateOnly startDate, DateOnly endDate)
    {
        var bookingDetails = await _bookingDetailRepository.GetByRoomIdAsync(roomId);

        return bookingDetails.All(bookingDetail =>
            bookingDetail.Booking.BookingStatus == BookingStatus.Completed ||
            bookingDetail.Booking.BookingStatus == BookingStatus.Cancelled ||
            bookingDetail.Booking.BookingStatus == BookingStatus.Pending ||
            startDate >= bookingDetail.Booking.CheckOutDate ||
            endDate <= bookingDetail.Booking.CheckInDate);
    }
}