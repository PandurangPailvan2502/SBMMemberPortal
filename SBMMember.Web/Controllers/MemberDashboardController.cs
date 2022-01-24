using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SBMMember.Data;
using SBMMember.Data.DataFactory;
using SBMMember.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Models;
namespace SBMMember.Web.Controllers
{
    public class MemberDashboardController : Controller
    {
        private readonly IMemberDataFactory memberDataFactory;
        private readonly IMemberPersonalDataFactory personalDataFactory;
        private readonly IMemberContactDetailsDataFactory contactDetailsDataFactory;
        private readonly IMemberEducationEmploymentDataFactory educationEmploymentDataFactory;
        private readonly IMemberFamilyDetailsDataFactory familyDetailsDataFactory;
        private readonly ILogger<MemberController> Logger;
        private readonly IMapper mapper;
        private static List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
        private readonly SBMMemberDBContext dBContext;
        private readonly IConfiguration configuration;
        public MemberDashboardController(IMemberDataFactory dataFactory,
            IMemberPersonalDataFactory memberPersonalDataFactory,
            IMemberContactDetailsDataFactory memberContactDetailsDataFactory,
            IMemberEducationEmploymentDataFactory memberEducationEmploymentDataFactory,
            IMemberFamilyDetailsDataFactory memberFamilyDetailsDataFactory,
            ILogger<MemberController> logger,
            IMapper Mapper,
            SBMMemberDBContext memberDBContext,
            IConfiguration _configuration)
        {
            Logger = logger;
            mapper = Mapper;
            memberDataFactory = dataFactory;
            personalDataFactory = memberPersonalDataFactory;
            contactDetailsDataFactory = memberContactDetailsDataFactory;
            educationEmploymentDataFactory = memberEducationEmploymentDataFactory;
            familyDetailsDataFactory = memberFamilyDetailsDataFactory;
            dBContext = memberDBContext;
            configuration = _configuration;

        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProfileUpdate()
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();

            var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            int MemberId = Convert.ToInt32(memberId);

            Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(MemberId);
            MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
            commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;

            Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(MemberId);
            MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
            commonViewModel.MemberConatctInfo = contactInfoViewModel;

            Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(MemberId);
            MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
            commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;

            List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(MemberId);
            List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
            foreach (Member_FamilyDetails family in member_Family)
            {
                memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
            }
            ViewBag.MemberList = memberFamilies;
            //List<MemberFamilyInfoViewModel> memberFamilies = mapper.Map<List<MemberFamilyInfoViewModel>>(member_Family);
            //MemberFamilyInfoViewModel familyInfoViewModel = new MemberFamilyInfoViewModel()
            //{
            //    MemberFamilyDetails = memberFamilies
            //};
            //commonViewModel.MemberFamilyInfo = familyInfoViewModel;
           
            return View(commonViewModel);
        }
    }
}
