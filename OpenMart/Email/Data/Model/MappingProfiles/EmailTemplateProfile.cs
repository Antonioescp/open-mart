using AutoMapper;
using OpenMart.Email.Data.Model.Template;
using OpenMart.Users.Data.Model;

namespace OpenMart.Email.Data.Model.MappingProfiles;

public class EmailTemplateProfile : Profile
{
    public EmailTemplateProfile()
    {
        this.CreateMap<User, UserEmailConfirmationTemplateModel>();
    }
}