using FluentValidation;
using TABP.Domain.Enums;
using TABP.Web.DTOs.Rooms;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Rooms;

public class CreateRoomValidator : AbstractValidator<CreateRoomDto>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("Room number is required")
            .ValidString(1, 8, "Room number");

        RuleFor(x => x.MaxChildren)
            .InclusiveBetween(0, 10).WithMessage("Max children must be between 0 and 10");

        RuleFor(x => x.MaxAdults)
            .NotEmpty().WithMessage("Max adults is required")
            .InclusiveBetween(1, 10).WithMessage("Max adults must be between 1 and 10");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .InclusiveBetween(1, 10000).WithMessage("Price must be between 1 and 10000");

        RuleFor(x => x.RoomType)
            .NotEmpty().WithMessage("Room type is required")
            .ValidEnumValue<CreateRoomDto, RoomType>();

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .ValidString(10, 150, "Description");
    }
}