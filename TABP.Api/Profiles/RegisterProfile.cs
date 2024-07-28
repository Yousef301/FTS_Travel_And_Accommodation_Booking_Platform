using AutoMapper;
using TABP.Application.Commands.Users.Register;
using TABP.Web.DTOs.Auth;

namespace TABP.Web.Profiles;

public class RegisterProfile : Profile
{
    public RegisterProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
    }
}