using FluentValidation;
using TABP.Web.DTOs.RoomAmenities;

namespace TABP.Web.Validators.RoomAmenities;

public class CreateRoomAmenityValidator : AbstractValidator<CreateRoomAmenityDto>
{
    public CreateRoomAmenityValidator()
    {
        RuleFor(x => x.AmenityId)
            .NotEmpty().WithMessage("Amenity Id is required.");
    }
}