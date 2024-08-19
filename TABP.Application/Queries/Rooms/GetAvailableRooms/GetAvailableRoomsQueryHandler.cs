using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Queries.Rooms.GetAvailableRooms;

public class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, IEnumerable<RoomResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetAvailableRoomsQueryHandler(IRoomRepository roomRepository,
        IHotelRepository hotelRepository,
        IMapper mapper)
    {
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomResponse>> Handle(GetAvailableRoomsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
        {
            throw new NotFoundException(nameof(Hotel), request.HotelId);
        }

        var rooms = await _roomRepository.GetByHotelIdAsync(request.HotelId,
            GetAvailableRoomsExpression(request.CheckInDate, request.CheckOutDate),
            includeAmenities: true);

        return _mapper.Map<IEnumerable<RoomResponse>>(rooms);
    }

    private Expression<Func<Room, bool>> GetAvailableRoomsExpression(DateOnly checkInDate,
        DateOnly checkOutDate)
    {
        return r => r.BookingDetails.All(bd =>
            bd.Booking.CheckOutDate <= checkInDate || bd.Booking.CheckInDate >= checkOutDate);
    }
}