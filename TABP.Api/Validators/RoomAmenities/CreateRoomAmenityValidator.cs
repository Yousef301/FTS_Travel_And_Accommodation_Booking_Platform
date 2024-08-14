using FluentValidation;
using TABP.Web.DTOs.RoomAmenities;

namespace TABP.Web.Validators.RoomAmenities;

public class CreateRoomAmenityValidator : AbstractValidator<CreateRoomAmenityDto>
{
    public CreateRoomAmenityValidator()
    {
        RuleFor(x => x.AmenitiesIds)
            .NotEmpty().WithMessage("Amenity list should contain at least one amenity ID.");
    }
}