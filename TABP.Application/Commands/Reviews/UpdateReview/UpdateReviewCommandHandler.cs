using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Reviews.UpdateReview;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateReviewCommandHandler(IReviewRepository reviewRepository,
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId) ??
                     throw new NotFoundException($"Review with id {request.ReviewId} wasn't found.");

        if (review.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You can't update this review.");
        }

        var oldReviewRate = review.Rate;

        var updatedReviewDto = _mapper.Map<ReviewUpdate>(review);

        request.ReviewDocument.ApplyTo(updatedReviewDto);

        _mapper.Map(updatedReviewDto, review);

        var newReviewRate = review.Rate;

        if (!IsReviewRateChanged(oldReviewRate, newReviewRate))
        {
            _reviewRepository.Update(review);

            await _unitOfWork.SaveChangesAsync();

            return;
        }

        var currentHotelRate = await _hotelRepository.GetHotelRateAsync(request.HotelId);
        var hotelReviewsCount = await _reviewRepository.GetHotelReviewsCount(request.HotelId);

        var newHotelRate = CalculateNewHotelRate(oldReviewRate, newReviewRate, currentHotelRate, hotelReviewsCount);

        try
        {
            _reviewRepository.Update(review);

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

    private double CalculateNewHotelRate(double oldReviewRate,
        double newReviewRate,
        double currentHotelRate,
        int hotelReviewsCount)
    {
        return (currentHotelRate * hotelReviewsCount - oldReviewRate + newReviewRate) / hotelReviewsCount;
    }

    private bool IsReviewRateChanged(double oldReviewRate,
        double newReviewRate)
    {
        return oldReviewRate != newReviewRate;
    }
}