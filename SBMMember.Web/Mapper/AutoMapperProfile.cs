using AutoMapper;
using SBMMember.Web.Models;
using SBMMember.Models;
using SBMMember.Models.MemberSearch;
using System.Collections.Generic;
using SBMMember.Web.Models.MemberSearchModels;
namespace SBMMember.Web.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MemberPerosnalInfoViewModel, Member_PersonalDetails>();
            CreateMap<Member_PersonalDetails, MemberPerosnalInfoViewModel>();


            CreateMap<MemberContactInfoViewModel, Member_ContactDetails>();
            CreateMap<Member_ContactDetails, MemberContactInfoViewModel>();

            CreateMap<MemberFamilyInfoViewModel, Member_FamilyDetails>();
            CreateMap<Member_FamilyDetails, MemberFamilyInfoViewModel>();
            // CreateMap<List< Member_FamilyDetails>,List< MemberFamilyInfoViewModel>>();

            CreateMap<MemberEducationEmploymentInfoViewModel, Member_EducationEmploymentDetails>();
            CreateMap<Member_EducationEmploymentDetails, MemberEducationEmploymentInfoViewModel>();

            CreateMap<MemberPaymentRecieptsViewModel, Member_PaymentsAndReciepts>();
            CreateMap<Member_PaymentsAndReciepts, MemberPaymentRecieptsViewModel>();

            CreateMap<MemberSearchResponseViewModel, MemberSearchResponse>();
            CreateMap<MemberSearchResponse, MemberSearchResponseViewModel>();

            CreateMap<MemberBusinessViewModel, Member_BusinessDetails>();
            CreateMap<Member_BusinessDetails, MemberBusinessViewModel>();

            CreateMap<JobPostingViewModel, JobPostings>();
            CreateMap<JobPostings, JobPostingViewModel>();

            CreateMap<EventAdsViewModel, EventAds>();
            CreateMap<EventAds, EventAdsViewModel>();

            CreateMap<AdminUsersViewModel, AdminUsers>();
            CreateMap<AdminUsers, AdminUsersViewModel>();

            CreateMap<UpcomingEventsViewModel, UpcomingEvent>();
            CreateMap<UpcomingEvent, UpcomingEventsViewModel>();

            CreateMap<EventTitlesViewModel, EventTitles>();
            CreateMap<EventTitles, EventTitlesViewModel>();

        }
    }
}
