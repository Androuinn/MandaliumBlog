using AutoMapper;
using Mandalium.API.Dtos;
using Mandalium.API.Models;
using System.Linq;

namespace Mandalium.API.Helpers
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


            CreateMap<BlogEntryForCreationDto, BlogEntry>();
            CreateMap<User,UserDto>();
            CreateMap<User,User>();
            CreateMap<UserDto,User>();
            CreateMap<Topic,TopicDto>();
            CreateMap<TopicDto,Topic>();


            CreateMap<Comment,CommentDtoForCreation>();
            CreateMap<CommentDtoForCreation,Comment>();

            CreateMap<Comment,CommentDto>().ForMember(dest => dest.CommenterName, opt => opt.MapFrom(src => (src.CommenterName == null) ? src.User.Username : src.CommenterName));
                

            CreateMap<UserForRegisterDto, User>();
            CreateMap<PhotoForCreationDto,Photo>();
            CreateMap<Photo,PhotoDto>();

           
        }
    }
}