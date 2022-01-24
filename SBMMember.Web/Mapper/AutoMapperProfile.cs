using AutoMapper;
using SBMMember.Web.Models;
using SBMMember.Models;
using System.Collections.Generic;

namespace SBMMember.Web.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MemberPerosnalInfoViewModel,Member_PersonalDetails>();
            CreateMap<Member_PersonalDetails, MemberPerosnalInfoViewModel>();

            CreateMap<MemberContactInfoViewModel, Member_ContactDetails>();
            CreateMap<Member_ContactDetails, MemberContactInfoViewModel>();

            CreateMap<MemberFamilyInfoViewModel, Member_FamilyDetails>();
            CreateMap<Member_FamilyDetails, MemberFamilyInfoViewModel>();
           // CreateMap<List< Member_FamilyDetails>,List< MemberFamilyInfoViewModel>>();

            CreateMap<MemberEducationEmploymentInfoViewModel, Member_EducationEmploymentDetails>();
            CreateMap<Member_EducationEmploymentDetails, MemberEducationEmploymentInfoViewModel>();
        }
    }
}
