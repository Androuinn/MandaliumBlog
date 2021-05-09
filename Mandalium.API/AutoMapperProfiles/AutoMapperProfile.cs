using AutoMapper;
using Mandalium.Core.Dto;
using Mandalium.Core.Models;

namespace Mandalium.API.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BlogEntry, BlogEntryDto>().ForMember(dest => dest.WriterName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.WriterSurname, opt => opt.MapFrom(src => src.User.Surname))
            .ForMember(dest => dest.WriterId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.TopicName));
            

            CreateMap<BlogEntry, BlogEntryListDto>()
            .ForMember(dest => dest.WriterName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.WriterSurname, opt => opt.MapFrom(src => src.User.Surname))
            .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.TopicName));

            CreateMap<User,UserDto>();
            CreateMap<User,User>();
            CreateMap<UserDto,User>();
            CreateMap<Topic,TopicDto>();
            CreateMap<TopicDto,Topic>();


            CreateMap<Comment,CommentDtoForCreation>();
            CreateMap<CommentDtoForCreation,Comment>()
                 .ForPath(dest => dest.User.Id, opt => opt.MapFrom(src => src.userId));

            CreateMap<Comment,CommentDto>()
            .ForMember(dest => dest.CommenterName, opt => opt.MapFrom(src => (src.CommenterName == null) ? src.User.Username : src.CommenterName))
            .ForMember(dest => dest.CommenterId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.User.PhotoUrl));
                

            CreateMap<UserForRegisterDto, User>();
            CreateMap<Photo,PhotoDto>();

           
        }
    }
}