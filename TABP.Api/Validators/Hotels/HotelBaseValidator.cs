using FluentValidation;
using TABP.Shared.Constants;
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
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Hotel name");

        RuleFor(x => x.Owner)
            .NotEmpty().WithMessage("Owner name is required")
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Owner name");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Address");

        RuleFor(x => x.Description)
            .NotEmpty()
            .ValidString(Constants.TextMinLength, Constants.TextMaxLength, "Description");
        ;
    }
}