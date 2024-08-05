using AutoMapper;
using MediatR;
using TABP.DAL.Enums;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

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
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

        if (booking == null)
        {
            throw new NotSupportedException("Booking not found");
        }

        if (booking.BookingStatus != BookingStatus.Confirmed)
        {
            throw new NotSupportedException("Booking cannot be cancelled");
        }

        booking.BookingStatus = BookingStatus.Cancelled;
        booking.PaymentStatus = PaymentStatus.Refunded;

        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();
    }
}