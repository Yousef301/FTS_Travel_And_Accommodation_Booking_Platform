using AutoMapper;
using TABP.Application.Commands.Users.Register;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class CredentialProfile : Profile
{
    public CredentialProfile()
    {
        CreateMap<RegisterCommand, Credential>();
    }
}