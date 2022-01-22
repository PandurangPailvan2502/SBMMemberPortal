using AutoMapper;
using SBMMember.Web.Models;
using SBMMember.Models;
namespace SBMMember.Web.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MemberPerosnalInfoViewModel,Member_PersonalDetails>();
            CreateMap<MemberContactInfoViewModel, Member_ContactDetails>();
            CreateMap<MemberFamilyInfoViewModel, Member_FamilyDetails>();
            CreateMap<MemberEducationEmploymentInfoViewModel, Member_EducationEmploymentDetails>();
            
        }
    }
}
