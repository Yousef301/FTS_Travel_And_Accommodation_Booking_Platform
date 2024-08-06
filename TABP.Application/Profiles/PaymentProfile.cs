using AutoMapper;
using TABP.Application.Queries.Payments;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, PaymentResponse>()
            .ForMember(dest => dest.PaymentStatus,
                opt =>
                    opt.MapFrom(src => src.PaymentStatus.ToString()));
    }
}