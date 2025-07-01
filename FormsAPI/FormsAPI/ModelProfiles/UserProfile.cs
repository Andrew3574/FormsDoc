using AutoMapper;
using FormsAPI.ModelsDTO.Account;
using FormsAPI.ModelsDTO.Users;
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

            CreateMap<User, UserDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dst => dst.State, opt => opt.MapFrom(src => src.State.ToString()))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dst => dst.Lastlogin, opt => opt.MapFrom(src => src.Lastlogin.ToString("O")))
                .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

        }
    }
}
