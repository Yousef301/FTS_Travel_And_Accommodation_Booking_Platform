using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Reviews.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IBookingRepository bookingRepository, IReviewRepository reviewRepository,
        IHotelRepository hotelRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        if (!await _bookingRepository.ExistsAsync(request.HotelId, request.UserId))
        {
            throw new NotSupportedException("User should have a booking for the hotel to be able to review");
        }

        if (await _reviewRepository.ExistsAsync(request.HotelId, request.UserId))
        {
            throw new NotSupportedException("User has already reviewed this hotel");
        }

        var currentHotelRate = await _hotelRepository.GetHotelRateAsync(request.HotelId);
        var hotelReviewsCount = await _reviewRepository.GetHotelReviewsCount(request.HotelId);

        var newRate = (currentHotelRate * hotelReviewsCount + request.Rate) / (hotelReviewsCount + 1);

        var createReview = _mapper.Map<Review>(request);
        createReview.Id = Guid.NewGuid();

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
            throw new NotSupportedException("An error occurred while creating a review. Please try again.");
        }
    }
}