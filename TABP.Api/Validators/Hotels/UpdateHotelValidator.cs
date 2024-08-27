using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Shared.Constants;
using TABP.Web.DTOs.Hotels;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Hotels;

public class UpdateHotelValidator : AbstractValidator<JsonPatchDocument<UpdateHotelDto>>
{
    public UpdateHotelValidator()
    {
        RuleForEach(x => x.Operations)
            .SetValidator(new JsonPatchOperationValidator());
    }

    private class JsonPatchOperationValidator : AbstractValidator<Operation<UpdateHotelDto>>
    {
        public JsonPatchOperationValidator()
        {
            RuleFor(x => x.path).NotEmpty();

            When(x => x.path.EndsWith("/CityId", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("City Id is required");
            });

            When(x => x.path.EndsWith("/Name", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Country name is required")
                    .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Name");
            });

            When(x => x.path.EndsWith("/Owner", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Post Office is required")
                    .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Owner");
            });

            When(x => x.path.EndsWith("/Address", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Address is required")
                    .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Address");
            });

            When(x => x.path.EndsWith("/Description", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Description is required")
                    .ValidString(Constants.TextMinLength, Constants.TextMaxLength, "Description");
            });
        }
    }
}