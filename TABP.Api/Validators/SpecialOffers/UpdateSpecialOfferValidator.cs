using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Web.DTOs.SpecialOffers;

namespace TABP.Web.Validators.SpecialOffers;

public class UpdateSpecialOfferValidator : AbstractValidator<JsonPatchDocument<UpdateSpecialOfferDto>>
{
    public UpdateSpecialOfferValidator()
    {
        RuleForEach(x => x.Operations)
            .SetValidator(new JsonPatchOperationValidator());

        RuleFor(x => x)
            .Must(IsValidDateRange)
            .WithMessage("End date must be greater than start date");
    }

    private bool IsValidDateRange(JsonPatchDocument<UpdateSpecialOfferDto> document)
    {
        var startDate = GetValueFromPatchDocument(document, "/StartDate");
        var endDate = GetValueFromPatchDocument(document, "/EndDate");

        if (DateTime.TryParse(startDate?.ToString(), out var startDateParsed) &&
            DateTime.TryParse(endDate?.ToString(), out var endDateParsed))
        {
            return endDateParsed > startDateParsed;
        }

        return true;
    }

    private static object? GetValueFromPatchDocument(JsonPatchDocument<UpdateSpecialOfferDto> document, string path)
    {
        return document.Operations
            .FirstOrDefault(op => op.path.Equals(path, StringComparison.OrdinalIgnoreCase))?
            .value;
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
                    .Must(value => Convert.ToDouble(value) >= 1 && Convert.ToDouble(value) <= 100)
                    .WithMessage("Discount must be between 1 and 100")
                    .WithName("Discount");
            });
        }
    }
}