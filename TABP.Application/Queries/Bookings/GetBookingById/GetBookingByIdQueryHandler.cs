using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Queries.Bookings.GetBookingById;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingResponse>
{
    private readonly IMapper _mapper;
    private readonly IBookingRepository _bookingRepository;

    public GetBookingByIdQueryHandler(IMapper mapper,
        IBookingRepository bookingRepository)
    {
        _mapper = mapper;
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingResponse> Handle(GetBookingByIdQuery request,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetDetailedByIdAsync(request.BookingId) ??
                      throw new NotFoundException(nameof(Booking), request.BookingId);

        if (booking.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You don't have permission to access this booking.");
        }

        return _mapper.Map<BookingResponse>(booking);
    }
}