using FluentValidation;
using TABP.Web.DTOs.Rooms;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Rooms;

public class GetAvailableRoomsValidator : AbstractValidator<GetAvailableRoomsDto>
{
    public GetAvailableRoomsValidator()
    {
        RuleFor(x => x.CheckInDate)
            .ValidFutureDate("Check in date", true);

        RuleFor(x => x.CheckOutDate)
            .ValidFutureDate("Check-out")
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after check-in date.");
    }
}