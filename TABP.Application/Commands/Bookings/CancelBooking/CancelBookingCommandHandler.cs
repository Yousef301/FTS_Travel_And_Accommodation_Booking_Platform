using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Bookings.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, request.UserId)
                      ?? throw new NotFoundException($"Booking with id {request.BookingId} wasn't found");

        if (booking.BookingStatus != BookingStatus.Confirmed)
        {
            throw new BookingStatusException("Booking cannot be cancelled");
        }

        booking.BookingStatus = BookingStatus.Cancelled;
        booking.PaymentStatus = PaymentStatus.Refunded;

        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();
    }
}