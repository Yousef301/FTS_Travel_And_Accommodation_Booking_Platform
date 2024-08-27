using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Enums;
using TABP.Shared.Exceptions;

namespace TABP.Application.Commands.Bookings.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, CancelBookingResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CancelBookingCommandHandler(IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CancelBookingResponse> Handle(CancelBookingCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, true)
                      ?? throw new NotFoundException(nameof(Booking), request.BookingId);

        if (booking.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You are not allowed to cancel this booking");
        }

        if (booking.BookingStatus != BookingStatus.Confirmed)
        {
            throw new BookingStatusException("Booking cannot be cancelled");
        }

        if (booking.CheckInDate <= DateOnly.FromDateTime(DateTime.Now))
        {
            throw new BookingStatusException("Booking cannot be cancelled it has already started");
        }

        booking.BookingStatus = BookingStatus.Cancelled;
        booking.PaymentStatus = PaymentStatus.Refunded;
        booking.Payment.PaymentStatus = PaymentStatus.Refunded;

        _bookingRepository.Update(booking);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CancelBookingResponse>(booking);
    }
}