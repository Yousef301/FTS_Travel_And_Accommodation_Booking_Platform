using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Shared.Constants;
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
                    .NotEmpty()
                    .WithMessage("Room Number is required")
                    .ValidString(Constants.MinimumRoomNumberLength, Constants.MaximumRoomNumberLength, "RoomNumber");
            });

            When(x => x.path.EndsWith("/MaxChildren", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty()
                    .WithMessage("Max children is required")
                    .Must(value =>
                        Convert.ToInt32(value) >= Constants.MinimumNumberOfChildren &&
                        Convert.ToInt32(value) <= Constants.MaximumNumberOfChildren)
                    .WithMessage($"Max children must be between {Constants.MinimumNumberOfChildren}" +
                                 $" and {Constants.MaximumNumberOfChildren}")
                    .WithName("MaxChildren");
            });

            When(x => x.path.EndsWith("/MaxAdults", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Max adults is required")
                    .Must(value =>
                        Convert.ToInt32(value) >= Constants.MinimumNumberOfAdults &&
                        Convert.ToInt32(value) <= Constants.MaximumNumberOfAdults)
                    .WithMessage($"Max adults must be between {Constants.MinimumNumberOfAdults}" +
                                 $" and {Constants.MaximumNumberOfAdults}")
                    .WithName("MaxAdults");
            });

            When(x => x.path.EndsWith("/Price", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.value)
                    .NotEmpty().WithMessage("Price is required")
                    .Must(value =>
                        Convert.ToDecimal(value) >= Constants.MinimumPrice &&
                        Convert.ToDecimal(value) <= Constants.MaximumPrice)
                    .WithMessage($"Price must be between {Constants.MinimumPrice}" +
                                 $" and {Constants.MaximumPrice}")
                    .WithName("Price");
            });
        }
    }
}