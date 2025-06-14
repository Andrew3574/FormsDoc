using AutoMapper;
using FormsAPI.ModelsDTO;
using FormsAPI.Services;
using Models;

namespace FormsAPI.ModelProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, User>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Passwordhash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Surname));
        }
    }
}
