using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Reviews.UpdateReview;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateReviewCommandHandler(IBookingRepository bookingRepository, IReviewRepository reviewRepository,
        IHotelRepository hotelRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId);

        if (!(review.UserId == request.UserId))
        {
            throw new NotSupportedException("You are not allowed to update this review.");
        }

        var oldReviewRate = review.Rate;

        var updatedReviewDto = _mapper.Map<ReviewUpdate>(review);

        request.ReviewDocument.ApplyTo(updatedReviewDto);

        _mapper.Map(updatedReviewDto, review);

        var newReviewRate = review.Rate;

        if (!IsReviewRateChanged(oldReviewRate, newReviewRate))
        {
            await _reviewRepository.UpdateAsync(review);

            await _unitOfWork.SaveChangesAsync();

            return;
        }

        var currentHotelRate = await _hotelRepository.GetHotelRateAsync(request.HotelId);
        var hotelReviewsCount = await _reviewRepository.GetHotelReviewsCount(request.HotelId);

        var newHotelRate = CalculateNewHotelRate(oldReviewRate, newReviewRate, currentHotelRate, hotelReviewsCount);

        try
        {
            await _reviewRepository.UpdateAsync(review);

            await _unitOfWork.SaveChangesAsync();

            await _hotelRepository.UpdateRateAsync(request.HotelId, Math.Round(newHotelRate, 2));

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new NotSupportedException("An error occurred while updating the review. Please try again.");
        }
    }

    private double CalculateNewHotelRate(double oldReviewRate, double newReviewRate, double currentHotelRate,
        int hotelReviewsCount)
    {
        return (currentHotelRate * hotelReviewsCount - oldReviewRate + newReviewRate) / hotelReviewsCount;
    }

    private bool IsReviewRateChanged(double oldReviewRate, double newReviewRate)
    {
        return oldReviewRate != newReviewRate;
    }
}