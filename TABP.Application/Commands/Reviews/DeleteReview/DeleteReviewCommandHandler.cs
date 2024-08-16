using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Reviews.DeleteReview;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(IReviewRepository reviewRepository,
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId) ??
                     throw new NotFoundException($"Review with id {request.ReviewId} wasn't found.");

        if (review.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You can't delete this review.");
        }

        var currentHotelRate = await _hotelRepository.GetHotelRateAsync(request.HotelId);
        var hotelReviewsCount = await _reviewRepository.GetHotelReviewsCount(request.HotelId);

        var newRate = (currentHotelRate * hotelReviewsCount - review.Rate) / (hotelReviewsCount - 1);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            _reviewRepository.Delete(review);

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