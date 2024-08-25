using FluentValidation;
using TABP.Domain.Constants;
using TABP.Web.DTOs.Reviews;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Reviews;

public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required")
            .ValidString(Constants.TextMinLength, Constants.TextMaxLength, "Comment");

        RuleFor(x => x.Rate)
            .NotEmpty().WithMessage("Rate is required")
            .InclusiveBetween(Constants.MinimumReviewRating, Constants.MaximumReviewRating)
            .WithMessage($"Rate must be between {Constants.MinimumReviewRating} and {Constants.MaximumReviewRating}");
    }
}