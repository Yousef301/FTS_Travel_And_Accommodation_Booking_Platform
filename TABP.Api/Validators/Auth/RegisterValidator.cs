using FluentValidation;
using TABP.Domain.Constants;
using TABP.Web.DTOs.Auth;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "First name");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .ValidString(Constants.NameMinLength, Constants.NameMaxLength, "Last name");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .ValidUsername(Constants.NameMinLength, Constants.NameMaxLength, "Username");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Your password cannot be empty")
            .ValidPassword(Constants.PasswordMinLength, Constants.PasswordMaxLength);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .ValidPhoneNumber();

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .ValidString(Constants.TextMinLength, Constants.TextMaxLength, "Address");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .ValidBirthDate();
    }
}