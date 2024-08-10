using FluentValidation;
using TABP.Web.DTOs.Hotels;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Hotels;

public class HotelBaseValidator : AbstractValidator<HotelBase>
{
    public HotelBaseValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("City Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Hotel name is required")
            .ValidName(3, 50, "Hotel name");

        RuleFor(x => x.Owner)
            .NotEmpty().WithMessage("Owner name is required")
            .ValidName(3, 50, "Owner name");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .ValidName(3, 50, "Address");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .ValidName(10, 150, "Description");;
    }
}