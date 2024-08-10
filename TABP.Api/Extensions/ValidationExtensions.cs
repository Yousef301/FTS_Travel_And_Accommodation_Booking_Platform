using FluentValidation;

namespace TABP.Web.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, object> ValidName<T>(this IRuleBuilder<T, string> ruleBuilder, int minLength,
        int maxLength, string propertyName)
    {
        return ruleBuilder
            .Matches("^[a-zA-Z]*$")
            .Length(minLength, maxLength)
            .WithMessage(
                $"{propertyName} must be between {minLength} and {maxLength} characters long and contain only letters.");
    }

    public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage($"Invalid phone number format.");
    }

    public static IRuleBuilderOptions<T, string> ValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder,
        int minLength, int maxLength)
    {
        return ruleBuilder
            .Length(minLength, maxLength).WithMessage("Your password length must be at least 8.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
    }

    public static IRuleBuilderOptions<T, DateOnly> ValidBirthDate<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
    {
        return ruleBuilder.LessThan(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Birth date must be in the past.");
    }

    public static IRuleBuilderOptions<T, string> ValidUsername<T>(this IRuleBuilder<T, string> ruleBuilder
        , int minLength, int maxLength, string propertyName)
    {
        return ruleBuilder
            .Length(minLength, maxLength)
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage(
                $"{propertyName} must be between {minLength} and {maxLength}" +
                $" characters and contain only letters and numbers.");
    }

    public static IRuleBuilderOptions<T, object> ValidName<T>(this IRuleBuilder<T, object> ruleBuilder, int minLength,
        int maxLength, string propertyName)
    {
        return ruleBuilder
            .Must(value => value.ToString().Length >= minLength)
            .WithMessage($"{propertyName} must be at least {minLength} characters")
            .Must(value => value.ToString().Length <= maxLength)
            .WithMessage($"{propertyName} must be less than or equal to {maxLength} characters.")
            .OverridePropertyName($"{propertyName}");
    }

    public static IRuleBuilderOptions<T, DateOnly> ValidFutureDate<T>(this IRuleBuilder<T, DateOnly> ruleBuilder,
        string propertyName, bool isToday = false)
    {
        if (isToday)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage($"{propertyName} must be today or in the future.");
        }

        return ruleBuilder
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage($"{propertyName} must be in the future.");
    }

    public static IRuleBuilderOptions<T, string> ValidEnumValue<T, TEnum>(this IRuleBuilder<T, string> ruleBuilder)
        where TEnum : struct, Enum
    {
        return ruleBuilder.Must(BeValidEnumValue<TEnum>)
            .WithMessage($"Value must be a valid {typeof(TEnum).Name}.");
    }

    private static bool BeValidEnumValue<TEnum>(string value) where TEnum : struct, Enum
    {
        return Enum.TryParse(typeof(TEnum), value, true, out _);
    }

    public static IRuleBuilderOptions<T, DateTime> ValidFutureDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder,
        string propertyName, bool isToday = false)
    {
        if (isToday)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage($"{propertyName} must be today or in the future.");
        }

        return ruleBuilder
            .GreaterThan(DateTime.Now)
            .WithMessage($"{propertyName} must be in the future.");
    }
}