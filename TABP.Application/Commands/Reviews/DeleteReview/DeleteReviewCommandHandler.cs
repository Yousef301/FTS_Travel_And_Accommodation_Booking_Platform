using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Reviews.DeleteReview;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteReviewCommandHandler(IBookingRepository bookingRepository, IReviewRepository reviewRepository,
        IHotelRepository hotelRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId);

        if (!(review.UserId == request.UserId))
        {
            throw new NotSupportedException("You are not allowed to delete this review.");
        }

        var currentHotelRate = await _hotelRepository.GetHotelRateAsync(request.HotelId);
        var hotelReviewsCount = await _reviewRepository.GetHotelReviewsCount(request.HotelId);

        var newRate = (currentHotelRate * hotelReviewsCount - review.Rate) / (hotelReviewsCount - 1);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _reviewRepository.DeleteAsync(request.ReviewId);

            await _unitOfWork.SaveChangesAsync();

            await _hotelRepository.UpdateRateAsync(request.HotelId, Math.Round(newRate, 2));

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new NotSupportedException("An error occurred while deleting a review. Please try again.");
        }
    }
}