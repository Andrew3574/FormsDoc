using AutoMapper;
using FormsAPP.Models.FormAnswers;
using FormsAPP.Models.FormAnswers.CRUD;

namespace FormsAPP.Profiles
{
    public class FormAnswerProfile : Profile
    {
        public FormAnswerProfile()
        {
            CreateMap<FormTemplateInfoModel, CreateFormAnswer>()
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.FormId))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.Version, opt => opt.MapFrom(src => src.Version))
                .ForMember(dst => dst.ShortTextAnswers, opt => opt.MapFrom(src => src.ShortTextAnswers))
                .ForMember(dst => dst.LongTextAnswers, opt => opt.MapFrom(src => src.LongTextAnswers))
                .ForMember(dst => dst.IntegerAnswers, opt => opt.MapFrom(src => src.IntegerAnswers))
                .ForMember(dst => dst.CheckboxAnswers, opt => opt.MapFrom(src => src.CheckboxAnswers));

        }
    }
}
