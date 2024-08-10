using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Web.DTOs.Reviews;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Reviews;

public class UpdateReviewValidator : AbstractValidator<JsonPatchDocument<UpdateReviewDto>>
{
    public UpdateReviewValidator()
    {
        RuleForEach(x => x.Operations)
            .SetValidator(new JsonPatchOperationValidator());
    }

    private class JsonPatchOperationValidator : AbstractValidator<Operation<UpdateReviewDto>>
    {
        public JsonPatchOperationValidator()
        {
            RuleFor(x => x.path).NotEmpty();

            When(x => x.path.EndsWith("/Comment", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Comment is required")
                    .ValidName(3, 120, "Name");
            });

            When(x => x.path.EndsWith("/Rate", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Rate is required")
                    .Must(value => value is >= 0 and <= 10)
                    .WithMessage("Rate must be between 0 and 10")
                    .WithName("Rate");
            });
        }
    }
}