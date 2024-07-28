using AutoMapper;
using TABP.Application.Commands.Users.Auth;
using TABP.Web.DTOs.Auth;

namespace TABP.Web.Profiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();
    }
}