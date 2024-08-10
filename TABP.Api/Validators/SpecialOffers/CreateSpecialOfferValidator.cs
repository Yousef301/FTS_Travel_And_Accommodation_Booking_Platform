﻿using FluentValidation;
using TABP.Web.DTOs.SpecialOffers;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.SpecialOffers;

public class CreateSpecialOfferValidator : AbstractValidator<CreateSpecialOfferDto>
{
    public CreateSpecialOfferValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .ValidFutureDate("Start date must be a future date", true);

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .ValidFutureDate("End date must be a future date")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be greater than start date");

        RuleFor(x => x.Discount)
            .NotEmpty().WithMessage("Discount is required")
            .InclusiveBetween(1, 100).WithMessage("Discount must be between 1 and 100");
    }
}