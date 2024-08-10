using FluentValidation;
using TABP.Web.DTOs.Hotels;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Hotels;

public class CreateHotelValidator : AbstractValidator<CreateHotelDto>
{
    public CreateHotelValidator()
    {
        Include(new HotelBaseValidator());

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .ValidPhoneNumber();

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.");
    }
}