using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignupDTO, APIUser>().ReverseMap()
               .ForAllMembers(opts => 
               opts.Condition((src, dest, srcMember) => srcMember != null)
               );
            CreateMap<StudentProfileDTO, Student>().ReverseMap()
               .ForAllMembers(opts =>
               opts.Condition((src, dest, srcMember) => srcMember != null)
               );

            CreateMap<CreateDeviceDTO, Device>().ReverseMap()
              .ForAllMembers(opts =>
              opts.Condition((src, dest, srcMember) => srcMember != null)
              );


            CreateMap<News, CreateNewsDTO>().ReverseMap()
              .ForAllMembers(opts =>
              opts.Condition((src, dest, srcMember) => srcMember != null)
              );
            CreateMap<News, UpdateNewsDTO>().ReverseMap()
             .ForAllMembers(opts =>
             opts.Condition((src, dest, srcMember) => srcMember != null)
             );
            CreateMap<News, NewsDTO>().ReverseMap()
             .ForAllMembers(opts =>
             opts.Condition((src, dest, srcMember) => srcMember != null)
             );

        }
    }
}
