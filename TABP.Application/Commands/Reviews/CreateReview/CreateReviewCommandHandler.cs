using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Enums;
using TABP.Shared.Exceptions;

namespace TABP.Application.Commands.Reviews.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IBookingRepository bookingRepository,
        IReviewRepository reviewRepository,
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _bookingRepository.ExistsAsync(b => b.UserId == request.UserId
                                                       && b.HotelId == request.HotelId
                                                       && b.BookingStatus == BookingStatus.Confirmed))
        {
            throw new NoBookingForHotelException("User should have a booking for the hotel to be able to review");
        }

        if (await _reviewRepository.ExistsAsync(r => r.HotelId == request.HotelId && r.UserId == request.UserId))
        {
            throw new AlreadyExistsException(
                "User has already reviewed this hotel, you can update the existed review instead.");
        }

        var currentHotelRate = await _hotelRepository.GetHotelRateAsync(request.HotelId);
        var hotelReviewsCount = await _reviewRepository.GetHotelReviewsCount(request.HotelId);

        var newRate = (currentHotelRate * hotelReviewsCount + request.Rate) / (hotelReviewsCount + 1);

        var createReview = _mapper.Map<Review>(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _reviewRepository.CreateAsync(createReview);

            await _unitOfWork.SaveChangesAsync();

            await _hotelRepository.UpdateRateAsync(request.HotelId, Math.Round(newRate, 2));

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}