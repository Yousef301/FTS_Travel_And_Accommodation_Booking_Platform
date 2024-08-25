using FluentValidation;
using TABP.Domain.Constants;
using TABP.Web.DTOs.Amenities;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Amenities;

public class CreateAmenityValidator : AbstractValidator<CreateAmenityDto>
{
    public CreateAmenityValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Amenity name is required")
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Amenity name");
    }
}