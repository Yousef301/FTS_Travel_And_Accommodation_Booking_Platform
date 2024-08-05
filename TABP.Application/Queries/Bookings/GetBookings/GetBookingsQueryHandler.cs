using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Bookings.GetBookings;

public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, IEnumerable<BookingResponse>>
{
    private readonly IMapper _mapper;
    private readonly IBookingRepository _bookingRepository;

    public GetBookingsQueryHandler(IMapper mapper, IBookingRepository bookingRepository)
    {
        _mapper = mapper;
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<BookingResponse>> Handle(GetBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetUserBookingsAsync(request.UserId);

        return _mapper.Map<IEnumerable<BookingResponse>>(bookings);
    }
}