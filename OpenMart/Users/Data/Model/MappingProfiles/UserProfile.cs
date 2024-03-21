using AutoMapper;
using OpenMart.Users.Data.Model.DTOs;

namespace OpenMart.Users.Data.Model.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRegistrationRequest, User>();
    }
}