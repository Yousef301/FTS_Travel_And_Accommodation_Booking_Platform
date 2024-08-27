using FluentValidation;
using TABP.Shared.Constants;
using TABP.Web.DTOs.Cities;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Cities;

public class CreateCityValidator : AbstractValidator<CreateCityDto>
{
    public CreateCityValidator()
    {
        RuleFor(city => city.Name)
            .NotEmpty().WithMessage("City name is required.")
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "City name");

        RuleFor(city => city.Country)
            .NotEmpty().WithMessage("Country is required.")
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Country name");

        RuleFor(city => city.PostOffice)
            .NotEmpty().WithMessage("Postal code is required.")
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Postal Code");
    }
}