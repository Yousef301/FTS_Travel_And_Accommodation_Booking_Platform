using FluentValidation;
using TABP.Shared.Constants;
using TABP.Shared.Enums;
using TABP.Web.DTOs.Rooms;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Rooms;

public class CreateRoomValidator : AbstractValidator<CreateRoomDto>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .WithMessage("Room number is required")
            .ValidString(Constants.MinimumRoomNumberLength, Constants.MaximumRoomNumberLength, "Room number");

        RuleFor(x => x.MaxChildren)
            .InclusiveBetween(Constants.MinimumNumberOfChildren, Constants.MaximumNumberOfChildren)
            .WithMessage($"Max children must be between {Constants.MinimumNumberOfChildren}" +
                         $" and {Constants.MaximumNumberOfChildren}");

        RuleFor(x => x.MaxAdults)
            .NotEmpty()
            .WithMessage("Max adults is required")
            .InclusiveBetween(Constants.MinimumNumberOfAdults, Constants.MaximumNumberOfAdults)
            .WithMessage($"Max adults must be between {Constants.MinimumNumberOfAdults}" +
                         $" and {Constants.MaximumNumberOfAdults}");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Price is required")
            .InclusiveBetween(Constants.MinimumPrice, Constants.MaximumPrice)
            .WithMessage($"Price must be between {Constants.MinimumPrice}" +
                         $" and {Constants.MaximumPrice}");

        RuleFor(x => x.RoomType)
            .NotEmpty().WithMessage("Room type is required")
            .ValidEnumValue<CreateRoomDto, RoomType>();

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .ValidString(Constants.TextMinLength, Constants.TextMaxLength, "Description");
    }
}