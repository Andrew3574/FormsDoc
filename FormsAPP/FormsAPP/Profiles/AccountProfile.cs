using AutoMapper;
using FormsAPP.Models.Account;
using FormsAPP.Models.Account.Salesforce;
using FormsAPP.Models.Users;

namespace FormsAPP.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterModel, LoginModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<UserModel, SalesforceContact>()
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
