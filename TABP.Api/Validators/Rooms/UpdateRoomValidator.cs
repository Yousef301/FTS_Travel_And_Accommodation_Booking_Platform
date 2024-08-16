using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Domain.Enums;
using TABP.Web.DTOs.Rooms;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Rooms;

public class UpdateRoomValidator : AbstractValidator<JsonPatchDocument<UpdateRoomDto>>
{
    public UpdateRoomValidator()
    {
        RuleForEach(x => x.Operations)
            .SetValidator(new JsonPatchOperationValidator());
    }

    private class JsonPatchOperationValidator : AbstractValidator<Operation<UpdateRoomDto>>
    {
        public JsonPatchOperationValidator()
        {
            RuleFor(x => x.path).NotEmpty();

            When(x => x.path.EndsWith("/RoomNumber", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Room Number is required")
                    .ValidString(1, 8, "RoomNumber");
            });

            When(x => x.path.EndsWith("/MaxChildren", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Max children is required")
                    .Must(value => Convert.ToInt32(value) >= 0 && Convert.ToInt32(value) <= 10)
                    .WithMessage("Max children must be between 0 and 10")
                    .WithName("MaxChildren");
            });

            When(x => x.path.EndsWith("/MaxAdults", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Max adults is required")
                    .Must(value => Convert.ToInt32(value) >= 1 && Convert.ToInt32(value) <= 10)
                    .WithMessage("Max adults must be between 1 and 10")
                    .WithName("MaxAdults");
            });

            When(x => x.path.EndsWith("/Price", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Price is required")
                    .Must(value => Convert.ToDecimal(value) >= 1 && Convert.ToDecimal(value) <= 10000)
                    .WithMessage("Price must be between 1 and 10000")
                    .WithName("Price");
            });
        }
    }
}