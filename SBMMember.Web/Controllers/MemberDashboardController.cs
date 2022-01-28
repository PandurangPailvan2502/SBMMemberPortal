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
using SBMMember.Web.Models.MemberSearchModels;
using SBMMember.Models.MemberSearch;

namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class MemberDashboardController : Controller
    {
        private readonly IMemberDataFactory memberDataFactory;
        private readonly IMemberPersonalDataFactory personalDataFactory;
        private readonly IMemberContactDetailsDataFactory contactDetailsDataFactory;
        private readonly IMemberEducationEmploymentDataFactory educationEmploymentDataFactory;
        private readonly IMemberFamilyDetailsDataFactory familyDetailsDataFactory;
        private readonly IMemberPaymentsDataFactory paymentsDataFactory;
        private readonly IMemberSearchDataFactory searchDataFactory;
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
            IMemberPaymentsDataFactory memberPaymentsDataFactory,
            IMemberSearchDataFactory memberSearchDataFactory,
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
            paymentsDataFactory = memberPaymentsDataFactory;
            searchDataFactory = memberSearchDataFactory;
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

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(MemberId);
            MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
            commonViewModel.MemberPaymentInfo = paymentViewModel;

            return View(commonViewModel);
        }
        [HttpPost]
        public IActionResult MemberPersonalInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_PersonalDetails member_Personal = mapper.Map<Member_PersonalDetails>(formCommonViewModel.MemberPersonalInfo);
            personalDataFactory.UpdateMemberPersonalDetails(member_Personal);

            return RedirectToAction("ProfileUpdate");
        }
        [HttpPost]
        public IActionResult MemberContactInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_ContactDetails member_Contact = mapper.Map<Member_ContactDetails>(formCommonViewModel.MemberConatctInfo);
            contactDetailsDataFactory.UpdateDetails(member_Contact);
            return RedirectToAction("ProfileUpdate");
        }
        [HttpPost]
        public IActionResult MemberEduEmpInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_EducationEmploymentDetails member_Education = mapper.Map<Member_EducationEmploymentDetails>(formCommonViewModel.MemberEducationEmploymentInfo);
            educationEmploymentDataFactory.UpdateDetails(member_Education);

            return RedirectToAction("ProfileUpdate");
        }

        public IActionResult MemberSearchDashBoard()
        {
            return View();
        }

        public IActionResult MemberSearchByName()
        {
            MemberSearchByNameViewModel viewModel = new MemberSearchByNameViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult MemberSearchByName(MemberSearchByNameViewModel model)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(model.FirstName))
                keyValuePairs.Add("FirstName", model.FirstName);
            if (!string.IsNullOrEmpty(model.MiddleName))
                keyValuePairs.Add("MiddleName", model.MiddleName);
            if (!string.IsNullOrEmpty(model.LastName))
                keyValuePairs.Add("LastName", model.LastName);

            List<MemberSearchResponse> memberData = searchDataFactory.GetAllMemberssSearchResultByFilterValues(keyValuePairs);
            List<MemberSearchResponseViewModel> filteredMember = new List<MemberSearchResponseViewModel>();
            foreach (MemberSearchResponse response in memberData)
            {
                MemberSearchResponseViewModel responseViewModel = mapper.Map<MemberSearchResponseViewModel>(response);
                filteredMember.Add(responseViewModel);
            }
            model.MemberList = filteredMember;
            return View(model);
        }
        public IActionResult MemberSearchByArea()
        {
            MemberSearchByAreaViewModel model = new MemberSearchByAreaViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult MemberSearchByArea(MemberSearchByAreaViewModel model)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(model.Area))
                keyValuePairs.Add("Area", model.Area);
            if (!string.IsNullOrEmpty(model.City))
                keyValuePairs.Add("City", model.City);
            if (!string.IsNullOrEmpty(model.District))
                keyValuePairs.Add("District", model.District);
            if (model.Pincode>0)
                keyValuePairs.Add("Pincode",Convert.ToString( model.Pincode));

            List<MemberSearchResponse> memberData = searchDataFactory.GetAllMemberssSearchResultByFilterValues(keyValuePairs);
            List<MemberSearchResponseViewModel> filteredMember = new List<MemberSearchResponseViewModel>();
            foreach (MemberSearchResponse response in memberData)
            {
                MemberSearchResponseViewModel responseViewModel = mapper.Map<MemberSearchResponseViewModel>(response);
                filteredMember.Add(responseViewModel);
            }
            model.MemberList = filteredMember;
            return View(model);
            
        }
        public IActionResult MemberSearchByNative()
        {
            return View();
        }
        public IActionResult MemberSearchByQualification()
        {
            return View();
        }
    }
}
