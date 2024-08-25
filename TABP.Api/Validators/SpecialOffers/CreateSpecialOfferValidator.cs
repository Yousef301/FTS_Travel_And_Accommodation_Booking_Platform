using FluentValidation;
using TABP.Domain.Constants;
using TABP.Web.DTOs.SpecialOffers;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.SpecialOffers;

public class CreateSpecialOfferValidator : AbstractValidator<CreateSpecialOfferDto>
{
    public CreateSpecialOfferValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .ValidFutureDate("Start date", true);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .ValidFutureDate("End date")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be greater than start date");

        RuleFor(x => x.Discount)
            .NotEmpty()
            .WithMessage("Discount is required")
            .InclusiveBetween(Constants.MinimumDiscount, Constants.MaximumDiscount)
            .WithMessage($"Discount must be between {Constants.MinimumDiscount} " +
                         $"and {Constants.MaximumDiscount}");
    }
}