using AutoMapper;
using FormsAPI.Models.FormAnswers;
using FormsAPI.ModelsDTO.FormAnswers;
using FormsAPI.ModelsDTO.FormAnswers.CRUD;
using FormsAPI.ModelsDTO.Forms;
using FormsAPI.ModelsDTO.Forms.CRUD_DTO;
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

            CreateMap<AnsweredFormTemplateDTO, FormAnswer>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.FormId))
                .ForMember(dst => dst.AsnweredAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dst => dst.CheckboxAnswers, opt => opt.MapFrom(src => src.CheckboxAnswers))
                .ForMember(dst => dst.IntegerAnswers, opt => opt.MapFrom(src => src.IntegerAnswers))
                .ForMember(dst => dst.ShortTextAnswers, opt => opt.MapFrom(src => src.ShortTextAnswers))
                .ForMember(dst => dst.LongTextAnswers, opt => opt.MapFrom(src => src.LongTextAnswers));

            CreateMap<CheckboxAnswerDTO, CheckboxAnswer>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
                .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<IntegerAnswerDTO, IntegerAnswer>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
                .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<ShortTextAnswerDTO, ShortTextAnswer>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
                .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<LongTextAnswerDTO, LongTextAnswer>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
                .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));


            //maps for sending
            CreateMap<FormAnswer, FormAnswerDTO>()
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.CheckboxAnswers, opt => opt.MapFrom(src => src.CheckboxAnswers))
                .ForMember(dst => dst.IntegerAnswers, opt => opt.MapFrom(src => src.IntegerAnswers))
                .ForMember(dst => dst.ShortTextAnswers, opt => opt.MapFrom(src => src.ShortTextAnswers))
                .ForMember(dst => dst.LongTextAnswers, opt => opt.MapFrom(src => src.LongTextAnswers));

            CreateMap<Form, FormTemplateDTO>()
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Questions, opt => opt.MapFrom(src => src.FormQuestions))
                .ForMember(dst => dst.Version, opt => opt.MapFrom(src => src.Version));

            CreateMap<FormAnswer, AnsweredFormTemplateDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.FormId))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Form!.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Form!.Description))
                .ForMember(dst => dst.Questions, opt => opt.MapFrom(src => src.Form!.FormQuestions))
                .ForMember(dst => dst.Version, opt => opt.MapFrom(src => src.Form!.Version))
                .ForMember(dst => dst.CheckboxAnswers, opt => opt.MapFrom(src => src.CheckboxAnswers))
                .ForMember(dst => dst.IntegerAnswers, opt => opt.MapFrom(src => src.IntegerAnswers))
                .ForMember(dst => dst.ShortTextAnswers, opt => opt.MapFrom(src => src.ShortTextAnswers))
                .ForMember(dst => dst.LongTextAnswers, opt => opt.MapFrom(src => src.LongTextAnswers));

            CreateMap<FormQuestion, FormQuestionDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.DisplayState, opt => opt.MapFrom(src => src.DisplayState))
                .ForMember(dst => dst.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.FormQuestionOptions));

            CreateMap<FormQuestionOption, FormQuestionOptionDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.OptionValue, opt => opt.MapFrom(src => src.OptionValue));

            CreateMap<FormAnswer, AnsweredFormDTO>()
                .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.Form, opt => opt.MapFrom(src => src.Form))
                .ForMember(dst => dst.AsnweredAt, opt => opt.MapFrom(src => src.AsnweredAt.ToString("D")));

            CreateMap<CheckboxAnswer, CheckboxAnswerDTO>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
               .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
               .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<IntegerAnswer, IntegerAnswerDTO>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
               .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
               .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<ShortTextAnswer, ShortTextAnswerDTO>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
               .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
               .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

            CreateMap<LongTextAnswer, LongTextAnswerDTO>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
               .ForMember(dst => dst.FormQuestionId, opt => opt.MapFrom(src => src.FormQuestionId))
               .ForMember(dst => dst.Answer, opt => opt.MapFrom(src => src.Answer));

        }
    }
}
