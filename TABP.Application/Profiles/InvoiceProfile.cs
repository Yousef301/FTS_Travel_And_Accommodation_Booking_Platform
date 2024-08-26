using AutoMapper;
using TABP.Application.Queries.Invoices;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, EmailInvoiceBody>()
            .ForMember(dest => dest.InvoiceId,
                opt =>
                    opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BookingId,
                opt =>
                    opt.MapFrom(src => src.BookingId))
            .ForMember(dest => dest.PaymentMethod,
                opt =>
                    opt.MapFrom(src => src.Booking.PaymentMethod.ToString()))
            .ForMember(dest => dest.PaymentStatus,
                opt =>
                    opt.MapFrom(src => src.PaymentStatus.ToString()))
            .ForMember(dest => dest.PaymentDate,
                opt =>
                    opt.MapFrom(src => src.InvoiceDate))
            .ForMember(dest => dest.InvoiceDate,
                opt =>
                    opt.MapFrom(src => src.InvoiceDate))
            .ForMember(dest => dest.TotalAmount,
                opt =>
                    opt.MapFrom(src => src.TotalPrice));
    }
}