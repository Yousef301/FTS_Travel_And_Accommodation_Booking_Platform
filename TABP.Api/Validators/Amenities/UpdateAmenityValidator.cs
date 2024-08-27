using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Shared.Constants;
using TABP.Web.DTOs.Amenities;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Amenities;

public class UpdateAmenityValidator : AbstractValidator<JsonPatchDocument<UpdateAmenityDto>>
{
    public UpdateAmenityValidator()
    {
        RuleForEach(x => x.Operations)
            .SetValidator(new JsonPatchOperationValidator());
    }

    private class JsonPatchOperationValidator : AbstractValidator<Operation<UpdateAmenityDto>>
    {
        public JsonPatchOperationValidator()
        {
            RuleFor(x => x.path).NotEmpty();

            When(x => x.path.EndsWith("/Name", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Name is required")
                    .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Name");
            });
        }
    }
}