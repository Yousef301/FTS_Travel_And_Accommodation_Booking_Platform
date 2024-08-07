using AutoMapper;
using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;

namespace TABP.Application.Commands.Bookings.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand>
{
    private readonly IBookingDetailRepository _bookingDetailRepository;
    private readonly IRoomBookingService _roomBookingService;
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBookingCommandHandler(IBookingDetailRepository bookingDetailRepository,
        IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IMapper mapper,
        IRoomBookingService roomBookingService, IRoomRepository roomRepository)
    {
        _bookingDetailRepository = bookingDetailRepository;
        _roomBookingService = roomBookingService;
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetByIdAndHotelIdAsync(request.HotelId,
            request.RoomIds);

        if (rooms.Count() != request.RoomIds.Count())
        {
            throw new ArgumentException("Rooms are not in the same hotel.");
        }

        var results = await ValidateRooms(request.RoomIds,
            request.CheckInDate, request.CheckOutDate);

        if (results.Any())
        {
            throw new ArgumentException(string.Join(Environment.NewLine, results));
        }

        if (await _bookingRepository.IsBookingOverlapsAsync(request.HotelId, request.UserId, request.CheckInDate,
                request.CheckOutDate))
        {
            throw new ArgumentException("Booking is overlapping with an existing booking.");
        }

        var totalPrice = CalculateTotalPrice(rooms);

        var paymentMethod = Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var parsedPaymentMethod)
            ? parsedPaymentMethod
            : throw new ArgumentException("Invalid payment method");

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

            foreach (var room in rooms)
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
                await _bookingRepository.DeleteAsync(pendingBooking);

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
        DateOnly checkInDate, DateOnly checkOutDate)
    {
        List<string> unavailableRooms = new List<string>();

        foreach (var roomId in roomIds)
        {
            var isRoomAvailable = await _roomBookingService.IsRoomAvailable(roomId, checkInDate, checkOutDate);
            if (!isRoomAvailable)
            {
                unavailableRooms.Add($"Room with id {roomId} is not available.");
            }
        }

        return unavailableRooms;
    }

    private double CalculateTotalPrice(IEnumerable<Room> rooms)
    {
        double totalPrice = 0;

        foreach (var room in rooms)
        {
            if (room.SpecialOffers.Any())
            {
                var specialOffer = room.SpecialOffers.First();
                totalPrice += room.Price - room.Price * specialOffer.Discount / 100;
            }
            else
            {
                totalPrice += room.Price;
            }
        }

        return totalPrice;
    }
}