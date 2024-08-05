// using AutoMapper;
// using MediatR;
// using TABP.DAL.Interfaces;
// using TABP.DAL.Interfaces.Repositories;
//
// namespace TABP.Application.Commands.Bookings.UpdateBooking;
//
// public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand>
// {
//     private readonly IBookingRepository _bookingRepository;
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public UpdateBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _bookingRepository = bookingRepository;
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     public async Task Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
//     {
//         var booking = await _bookingRepository.GetDetailedByIdAsync(request.BookingId);
//
//         if (booking == null)
//         {
//             throw new Exception("Booking not found"); // TODO: Create a custom exception
//         }
//
//         var updatedBookingDto = _mapper.Map<BookingUpdate>(booking);
//
//         request.BookingDocument.ApplyTo(updatedBookingDto);
//
//         _mapper.Map(updatedBookingDto, booking);
//
//         await _bookingRepository.UpdateAsync(booking);
//
//         await _unitOfWork.SaveChangesAsync();
//     }
// }