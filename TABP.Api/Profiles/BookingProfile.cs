using AutoMapper;
using TABP.Application.Commands.Bookings.CreateBooking;
using TABP.Web.DTOs.Bookings;

namespace TABP.Web.Profiles;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<CreateBookingDto, CreateBookingCommand>();
    }
}