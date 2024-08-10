using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Web.DTOs.Cities;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Cities;

public class UpdateCityValidator : AbstractValidator<JsonPatchDocument<UpdateCityDto>>
{
    public UpdateCityValidator()
    {
        RuleForEach(x => x.Operations)
            .SetValidator(new JsonPatchOperationValidator());
    }

    private class JsonPatchOperationValidator : AbstractValidator<Operation<UpdateCityDto>>
    {
        public JsonPatchOperationValidator()
        {
            RuleFor(x => x.path).NotEmpty();

            When(x => x.path.EndsWith("/Name", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("City name is required")
                    .ValidName(2, 50, "Name");
            });

            When(x => x.path.EndsWith("/Country", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Country name is required")
                    .ValidName(2, 50, "Country");
            });
            
            When(x => x.path.EndsWith("/PostOffice", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Post Office is required")
                    .ValidName(3, 30, "Post Office");

            });
        }
    }
}