using AutoMapper;
using TABP.Application.Queries.Bookings;
using TABP.DAL.Entities;
using TABP.DAL.Models;

namespace TABP.Application.Profiles;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<BookingDto, BookingResponse>()
            .ForMember(dest => dest.BookingStatus
                , opt => opt.MapFrom(
                    src => src.BookingStatus.ToString()))
            .ForMember(dest => dest.PaymentMethod
                , opt => opt.MapFrom(
                    src => src.PaymentMethod.ToString()))
            .ForMember(dest => dest.PaymentStatus
                , opt => opt.MapFrom(
                    src => src.PaymentStatus.ToString()));
    }
}