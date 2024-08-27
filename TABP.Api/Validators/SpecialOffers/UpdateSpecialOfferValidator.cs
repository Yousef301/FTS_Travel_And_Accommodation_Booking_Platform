using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Shared.Constants;
using TABP.Web.DTOs.SpecialOffers;

namespace TABP.Web.Validators.SpecialOffers;

public class UpdateSpecialOfferValidator : AbstractValidator<JsonPatchDocument<UpdateSpecialOfferDto>>
{
    public UpdateSpecialOfferValidator()
    {
        RuleForEach(x => x.Operations)
            .SetValidator(new JsonPatchOperationValidator());
    }

    private class JsonPatchOperationValidator : AbstractValidator<Operation<UpdateSpecialOfferDto>>
    {
        public JsonPatchOperationValidator()
        {
            RuleFor(x => x.path).NotEmpty();

            When(x => x.path.EndsWith("/Discount", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Discount is required")
                    .Must(value =>
                        Convert.ToDouble(value) >= Constants.MinimumDiscount &&
                        Convert.ToDouble(value) <= Constants.MaximumDiscount)
                    .WithMessage($"Discount must be between {Constants.MinimumDiscount}" +
                                 $" and {Constants.MaximumDiscount}")
                    .WithName("Discount");
            });
        }
    }
}