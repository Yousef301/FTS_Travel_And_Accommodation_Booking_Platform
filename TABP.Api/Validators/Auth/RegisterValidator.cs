using FluentValidation;
using TABP.Web.DTOs.Auth;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .ValidString(3, 50, "First name");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .ValidString(3, 50, "Last name");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .ValidUsername(3, 50, "Username");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Your password cannot be empty")
            .ValidPassword(8, 50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .ValidPhoneNumber();

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .ValidString(3, 100, "Address");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .ValidBirthDate();
    }
}