using AutoMapper;
using FormsAPI.Models.FormAnswers;
using FormsAPI.ModelsDTO.FormAnswers;
using FormsAPI.ModelsDTO.Forms;
using Models;

namespace FormsAPI.ModelProfiles
{
    public class FormAnswersProfile : Profile
    {
        public FormAnswersProfile()
        {
            //maps for creating
            CreateMap<CreateFormAnswerDTO, FormAnswer>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.FormId))
                .ForMember(dst => dst.AsnweredAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dst => dst.CheckboxAnswers, opt => opt.MapFrom(src => src.CheckboxAnswers))
                .ForMember(dst => dst.IntegerAnswers, opt => opt.MapFrom(src => src.IntegerAnswers))
                .ForMember(dst => dst.ShortTextAnswers, opt => opt.MapFrom(src => src.ShortTextAnswers))
                .ForMember(dst => dst.LongTextAnswers, opt => opt.MapFrom(src => src.LongTextAnswers));

            CreateMap<CheckboxAnswerDTO, CheckboxAnswer>()
                .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
                .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<IntegerAnswerDTO, IntegerAnswer>()
                .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
                .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<ShortTextAnswerDTO, ShortTextAnswer>()
                .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
                .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<LongTextAnswerDTO, LongTextAnswer>()
                .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
                .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            //maps for sending
            CreateMap<Form, FormTemplateInfoDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Questions, opt => opt.MapFrom(src => src.FormQuestions));

            CreateMap<FormQuestion, FormQuestionDTO>()
                .ForMember(dst => dst.QuestionText, opt => opt.MapFrom(src => src.Question))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.DisplayState, opt => opt.MapFrom(src => src.DisplayState))
                .ForMember(dst => dst.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.FormQuestionOptions));

            CreateMap<FormQuestionOption, FormQuestionOptionDTO>()
                .ForMember(dst => dst.OptionValue, opt => opt.MapFrom(src => src.OptionValue));

        }
    }
}
