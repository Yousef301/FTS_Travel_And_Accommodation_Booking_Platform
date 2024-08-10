using FluentValidation;
using TABP.Web.DTOs.Reviews;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Reviews;

public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required")
            .ValidName(3, 120, "Comment");

        RuleFor(x => x.Rate)
            .NotEmpty().WithMessage("Rate is required")
            .InclusiveBetween(0, 10).WithMessage("Rate must be between 0 and 10");
    }
}