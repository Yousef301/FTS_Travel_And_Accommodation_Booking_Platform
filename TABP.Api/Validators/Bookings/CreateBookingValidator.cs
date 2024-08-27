using FluentValidation;
using TABP.Shared.Enums;
using TABP.Web.DTOs.Bookings;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Bookings;

public class CreateBookingValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty()
            .WithMessage("Hotel ID must be provided.");

        RuleFor(x => x.RoomIds)
            .NotEmpty()
            .WithMessage("At least one room ID must be provided.");

        RuleFor(x => x.CheckInDate)
            .ValidFutureDate("Check-in", true);

        RuleFor(x => x.CheckOutDate)
            .ValidFutureDate("Check-out")
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after check-in date.");

        RuleFor(x => x.PaymentMethod)
            .ValidEnumValue<CreateBookingDto, PaymentMethod>();
    }
}