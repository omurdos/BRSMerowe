using AutoMapper;
using Core.Entities;
using Dashboard.ViewModel;

namespace Dashboard.MappingProfiles
{
  public class ViewModelsMappingProfiles : Profile
  {
    public ViewModelsMappingProfiles()
    {
      CreateMap<PushNotification, PushNotificationViewModel>().ReverseMap()
         .ForAllMembers(opts =>
         opts.Condition((src, dest, srcMember) => srcMember != null)
         );
      CreateMap<PushNotification, CreatePushNotificationViewModel>().ReverseMap()
        .ForAllMembers(opts =>
        opts.Condition((src, dest, srcMember) => srcMember != null)
        );
      CreateMap<PushNotification, CreateIndividualPushNotificationViewModel>().ReverseMap()
     .ForAllMembers(opts =>
     opts.Condition((src, dest, srcMember) => srcMember != null)
     );
      CreateMap<APIUser, UserViewModel>().ReverseMap()
     .ForAllMembers(opts =>
     opts.Condition((src, dest, srcMember) => srcMember != null)
     );
      CreateMap<APIUser, CreateUserViewModel>().ReverseMap()
    .ForAllMembers(opts =>
    opts.Condition((src, dest, srcMember) => srcMember != null)
    );
            CreateMap<APIUser, EditUserViewModel>().ReverseMap()
.ForAllMembers(opts =>
opts.Condition((src, dest, srcMember) => srcMember != null)
);
            CreateMap<Service, ServiceViewModel>().ReverseMap()
     .ForAllMembers(opts =>
     opts.Condition((src, dest, srcMember) => srcMember != null)
     );
      CreateMap<Announcement, AnnouncementViewModel>().ReverseMap()
      .ForAllMembers(opts =>
      opts.Condition((src, dest, srcMember) => srcMember != null)
      );
      CreateMap<Announcement, EditAnnouncementViewModel>().ReverseMap()
    .ForAllMembers(opts =>
    opts.Condition((src, dest, srcMember) => srcMember != null)
    );
      CreateMap<CertificateRequest, CertificateRequestViewModel>().ReverseMap()
      .ForAllMembers(opts =>
      opts.Condition((src, dest, srcMember) => srcMember != null)
      );
      CreateMap<CertificateRequest, EditCertificateRequestViewModel>().ReverseMap()
    .ForAllMembers(opts =>
    opts.Condition((src, dest, srcMember) => srcMember != null)
    );
     CreateMap<EnrollmentRequest, CertificateRequestViewModel>().ReverseMap()
      .ForAllMembers(opts =>
      opts.Condition((src, dest, srcMember) => srcMember != null)
      );
      CreateMap<EnrollmentRequest, EditCertificateRequestViewModel>().ReverseMap()
    .ForAllMembers(opts =>
    opts.Condition((src, dest, srcMember) => srcMember != null)
    );
     CreateMap<TranscriptRequest, CertificateRequestViewModel>().ReverseMap()
      .ForAllMembers(opts =>
      opts.Condition((src, dest, srcMember) => srcMember != null)
      );
      CreateMap<TranscriptRequest, EditCertificateRequestViewModel>().ReverseMap()
    .ForAllMembers(opts =>
    opts.Condition((src, dest, srcMember) => srcMember != null)
    );
      CreateMap<StudentSubject, StudentSubjectViewModel>().ReverseMap()
.ForAllMembers(opts =>
opts.Condition((src, dest, srcMember) => srcMember != null)
);



      CreateMap<SystemClaim, SystemClaimViewModel>().ReverseMap()
      .ForAllMembers(opts =>
      opts.Condition((src, dest, srcMember) => srcMember != null)
      );
    }
  }
}
