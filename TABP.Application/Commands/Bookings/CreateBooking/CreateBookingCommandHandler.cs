using AutoMapper;
using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Bookings.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand>
{
    private readonly IBookingDetailRepository _bookingDetailRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(IBookingDetailRepository bookingDetailRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IRoomRepository roomRepository,
        IHotelRepository hotelRepository)
    {
        _bookingDetailRepository = bookingDetailRepository;
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
        {
            throw new NotFoundException($"Hotel with id {request.HotelId} wasn't found");
        }

        var rooms = await _roomRepository.GetByIdAndHotelIdAsync(request.HotelId, request.RoomIds);

        var roomsList = rooms.ToList();

        if (roomsList.Count != request.RoomIds.Count())
        {
            throw new NotFoundException($"One or more rooms doesn't exist in the hotel with id {request.HotelId}");
        }

        if (await _bookingRepository.IsBookingOverlapsAsync(request.HotelId, request.UserId, request.CheckInDate,
                request.CheckOutDate))
        {
            throw new BookingOverlapException();
        }

        var results = await ValidateRooms(request.RoomIds, request.CheckInDate, request.CheckOutDate);
        var resultsList = results.ToList();

        if (resultsList.Count != 0)
        {
            throw new RoomsNotAvailableException(resultsList);
        }

        var totalPrice = CalculateTotalPrice(roomsList);

        var paymentMethod = Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var parsedPaymentMethod)
            ? parsedPaymentMethod
            : throw new InvalidPaymentMethodException();

        var pendingBooking = await _bookingRepository.GetPendingBooking(request.UserId);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                HotelId = request.HotelId,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                PaymentStatus = PaymentStatus.Pending,
                BookingStatus = BookingStatus.Pending,
                PaymentMethod = paymentMethod,
                TotalPrice = totalPrice,
            };

            await _bookingRepository.CreateAsync(booking);

            await _unitOfWork.SaveChangesAsync();

            foreach (var room in roomsList)
            {
                var bookingDetail = new BookingDetail
                {
                    Id = Guid.NewGuid(),
                    BookingId = booking.Id,
                    RoomId = room.Id
                };

                await _bookingDetailRepository.CreateAsync(bookingDetail);
            }

            if (pendingBooking != null)
                _bookingRepository.Delete(pendingBooking);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task<IEnumerable<string>?> ValidateRooms(IEnumerable<Guid> roomIds,
        DateOnly checkInDate,
        DateOnly checkOutDate)
    {
        List<string> unavailableRooms = new List<string>();

        foreach (var roomId in roomIds)
        {
            var isRoomAvailable =
                await _bookingDetailRepository.IsRoomAvailableAsync(roomId, checkInDate, checkOutDate);
            if (!isRoomAvailable)
            {
                unavailableRooms.Add($"Room with id {roomId} is not available.");
            }
        }

        return unavailableRooms;
    }

    private decimal CalculateTotalPrice(IEnumerable<Room> rooms)
    {
        decimal totalPrice = 0;

        foreach (var room in rooms)
        {
            if (room.SpecialOffers.Any())
            {
                var specialOffer = room.SpecialOffers.First();
                totalPrice += room.Price - room.Price * Convert.ToDecimal(specialOffer.Discount) / 100;
            }
            else
            {
                totalPrice += room.Price;
            }
        }

        return totalPrice;
    }
}