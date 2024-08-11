using FluentValidation;
using TABP.Web.DTOs.Cities;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Cities;

public class CreateCityValidator : AbstractValidator<CreateCityDto>
{
    public CreateCityValidator()
    {
        RuleFor(city => city.Name)
            .NotEmpty().WithMessage("City name is required.")
            .ValidString(2, 50, "City name");

        RuleFor(city => city.Country)
            .NotEmpty().WithMessage("Country is required.")
            .ValidString(2, 50, "Country name");

        RuleFor(city => city.PostOffice)
            .NotEmpty().WithMessage("Postal code is required.")
            .ValidString(3, 30, "Postal Code");
    }
}