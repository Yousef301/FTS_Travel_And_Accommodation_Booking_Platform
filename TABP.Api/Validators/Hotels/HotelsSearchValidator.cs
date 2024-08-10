using FluentValidation;
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
            .InclusiveBetween(1, 30).WithMessage("Number of rooms must be between 0 and 10");

        RuleFor(x => x.NumberOfAdults)
            .InclusiveBetween(2, 15).WithMessage("Number of adults must between 2 and 15");

        RuleFor(x => x.NumberOfChildren)
            .InclusiveBetween(0, 15).WithMessage("Number of children must between 0 and 15");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum price must be at least 0.")
            .LessThan(x => x.MaxPrice).WithMessage("Minimum price must be less than maximum price.");

        RuleFor(x => x.MaxPrice)
            .GreaterThan(x => x.MinPrice).WithMessage("Maximum price must be greater than minimum price.")
            .LessThanOrEqualTo(10000).WithMessage("Maximum price must be at most 1000.");

        RuleFor(x => x.ReviewRating)
            .InclusiveBetween(0, 10).WithMessage("Rate must be between 0 and 10");
    }
}