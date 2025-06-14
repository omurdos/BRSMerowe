using AutoMapper;
using Core.Entities;
using ERegistration.Data;

namespace ERegistration.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<BasicInfo, Student>().ReverseMap()
               .ForAllMembers(opts =>
               opts.Condition((src, dest, srcMember) => srcMember != null)
               );
            CreateMap<Guardian, GuardianInfo>().ReverseMap()
                .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null)
                );
        }
    }
}
