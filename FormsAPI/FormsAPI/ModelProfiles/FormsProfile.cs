using AutoMapper;
using FormsAPI.ModelsDTO.Forms;
using Models;
using Models.Enums;
using OnixLabs.Core.Linq;

namespace FormsAPI.ModelProfiles
{
    public class FormsProfile : Profile
    {
        public FormsProfile()
        {
            //maps for creating
            CreateMap<CreateFormDTO, Form>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dst => dst.TopicId, opt => opt.MapFrom(src => src.TopicId))
                .ForMember(dst => dst.Accessibility, opt => opt.MapFrom(src => (FormAccessibility)src.Accessibility))
                .ForMember(dst => dst.FormQuestions, opt => opt.MapFrom(src => src.Questions))
                .ForMember(dst => dst.AccessformUsers, opt => opt.MapFrom(src => src.AccessUsers));

            CreateMap<FormQuestionDTO, FormQuestion>()
                .ForMember(dst => dst.Question, opt => opt.MapFrom(src => src.QuestionText))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.DisplayState, opt => opt.MapFrom(src => src.DisplayState))
                .ForMember(dst => dst.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dst => dst.FormQuestionOptions, opt => opt.MapFrom(src => src.Options));

            CreateMap<FormQuestionOptionDTO, FormQuestionOption>()
                .ForMember(dst => dst.OptionValue, opt => opt.MapFrom(src => src.OptionValue));
            
            CreateMap<AccessformUserDTO, AccessformUser>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<LikeDTO, Like>()
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.FormId))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<CreateCommentDTO, Comment>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.FormId))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Text));

            //maps for sending
            CreateMap<Form, FormDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("D")))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dst => dst.Topic, opt => opt.MapFrom(src => src.Topic!.Name))
                .ForMember(dst => dst.Accessibility, opt => opt.MapFrom(src => src.Accessibility.ToString()))
                .ForMember(dst => dst.Tags, opt => opt.MapFrom(src => src.FormTags.Select(ft => ft.Tag!.Name)))
                .ForMember(dst => dst.LikesCount, opt => opt.MapFrom(src => src.Likes.Count()))
                .ForMember(dst => dst.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<Comment, CommentDTO>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.User!.Name))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("MMMM dd, yyyy")));

            CreateMap<User, UserFormDTO>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Surname));

            CreateMap<Tag, FilterTagDTO>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<User, FilterUserDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
