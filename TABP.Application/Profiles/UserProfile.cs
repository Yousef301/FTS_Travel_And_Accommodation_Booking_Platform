using AutoMapper;
using TABP.Application.Commands.Users.Register;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterCommand, User>();
    }
}