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
            CreateMap<BlogEntry, BlogEntryDto>().ForMember(dest => dest.WriterName, opt => opt.MapFrom(src => src.Writer.Name))
            .ForMember(dest => dest.WriterSurname, opt => opt.MapFrom(src => src.Writer.Surname))
            .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.TopicName));
            

            CreateMap<BlogEntry, BlogEntryListDto>()
            .ForMember(dest => dest.WriterName, opt => opt.MapFrom(src => src.Writer.Name))
            .ForMember(dest => dest.WriterSurname, opt => opt.MapFrom(src => src.Writer.Surname))
            .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.TopicName));


            CreateMap<BlogEntryForCreationDto, BlogEntry>();
            CreateMap<Writer,WriterDto>();
            CreateMap<Topic,TopicDto>();
            CreateMap<TopicDto,Topic>();


            CreateMap<Comment,CommentDtoForCreation>();
            CreateMap<CommentDtoForCreation,Comment>();

            CreateMap<Comment,CommentDto>();

            CreateMap<WriterForRegisterDto, Writer>();
            CreateMap<PhotoForCreationDto,Photo>();

           
        }
    }
}