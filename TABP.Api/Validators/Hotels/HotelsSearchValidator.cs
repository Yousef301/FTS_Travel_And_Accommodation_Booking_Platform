using FluentValidation;
using TABP.Domain.Constants;
using TABP.Domain.Enums;
using TABP.Web.DTOs.Hotels;
using TABP.Web.Extensions;
using TABP.Web.Validators.Filters;

namespace TABP.Web.Validators.Hotels;

public class HotelsSearchValidator : AbstractValidator<HotelsSearchDto>
{
    public HotelsSearchValidator()
    {
        Include(new FilterParametersValidator());

        RuleFor(x => x.CheckInDate)
            .ValidFutureDate("Check-in date", true);

        RuleFor(x => x.CheckOutDate)
            .ValidFutureDate("Check-out")
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after check-in date.");

        RuleFor(x => x.NumberOfRooms)
            .InclusiveBetween(Constants.MinimumNumberOfRooms, Constants.MaximumNumberOfRooms)
            .WithMessage($"Number of rooms must be between {Constants.MinimumNumberOfRooms} " +
                         $"and {Constants.MaximumNumberOfRooms}");

        RuleFor(x => x.NumberOfAdults)
            .InclusiveBetween(Constants.MinimumNumberOfAdults, Constants.MaximumNumberOfAdults)
            .WithMessage($"Number of adults must between {Constants.MinimumNumberOfAdults}" +
                         $" and {Constants.MaximumNumberOfAdults}");

        RuleFor(x => x.NumberOfChildren)
            .InclusiveBetween(Constants.MinimumNumberOfChildren, Constants.MaximumNumberOfChildren)
            .WithMessage($"Number of children must between {Constants.MinimumNumberOfChildren}" +
                         $" and {Constants.MaximumNumberOfChildren}");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(Constants.MinimumPrice)
            .WithMessage($"Minimum price must be at least {Constants.MinimumPrice}.")
            .LessThan(x => x.MaxPrice)
            .WithMessage("Minimum price must be less than maximum price.");

        RuleFor(x => x.MaxPrice)
            .GreaterThan(x => x.MinPrice)
            .WithMessage("Maximum price must be greater than minimum price.")
            .LessThanOrEqualTo(Constants.MaximumPrice)
            .WithMessage($"Maximum price must be at most {Constants.MaximumPrice}.");

        RuleFor(x => x.RoomType)
            .ValidEnumValue<HotelsSearchDto, RoomType>();

        RuleFor(x => x.ReviewRating)
            .InclusiveBetween(Constants.MinimumReviewRating, Constants.MaximumReviewRating)
            .WithMessage($"Rate must be between {Constants.MinimumReviewRating} and {Constants.MaximumReviewRating}");
    }
}