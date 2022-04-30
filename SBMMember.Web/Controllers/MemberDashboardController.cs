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
using SBMMember.Web.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

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
            commonViewModel.ProfilePercentage = 0;
            Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(MemberId);
            if (member_Personal.MemberId > 0)
            {
                MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
                commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;
                commonViewModel.ProfilePercentage += 20;
            }

            Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(MemberId);
            if (member_contact.MemberId > 0)
            {
                MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
                commonViewModel.MemberConatctInfo = contactInfoViewModel;
                commonViewModel.ProfilePercentage += 20;
            }
            Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(MemberId);
            if (member_education.MemberId > 0)
            {
                MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
                commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;
                commonViewModel.ProfilePercentage += 20;
            }

            List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(MemberId);
            List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
            foreach (Member_FamilyDetails family in member_Family)
            {
                memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
            }
            //ViewBag.MemberList = memberFamilies;
            MemberFamilyInfoViewModel familyInfoview = new MemberFamilyInfoViewModel();
            familyInfoview.MemberFamilyDetails = memberFamilies;
            familyInfoview.MemberId = MemberId;

            commonViewModel.MemberFamilyInfo = familyInfoview;
            if(memberFamilies.Count>0)
                commonViewModel.ProfilePercentage += 20;

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(MemberId);
            if (member_Payments.MemberId > 0)
            {
                MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
                commonViewModel.MemberPaymentInfo = paymentViewModel;
                commonViewModel.ProfilePercentage += 20;
            }

            return View(commonViewModel);
        }
        public IActionResult EditFamilyMember(int id)
        {
            Member_FamilyDetails member_Family = familyDetailsDataFactory.GetDetailsByMemberId(id);
            MemberFamilyInfoViewModel model = mapper.Map<MemberFamilyInfoViewModel>(member_Family);

            return RedirectToAction("ProfileUpdate");
        }
        public IActionResult DeleteFamilyMember(int id, int memberId)
        {
            familyDetailsDataFactory.DeleteById(id);

            return RedirectToAction("ProfileUpdate");
        }
        public IActionResult ViewLoggedMemberProfile()
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();

            var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            int MemberId = Convert.ToInt32(memberId);
            commonViewModel.ProfilePercentage=0;
            Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(MemberId);
            if (member_Personal.MemberId > 0)
            {
                MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
                commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;
                commonViewModel.ProfilePercentage += 20;
            }

            Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(MemberId);
            if (member_contact.MemberId > 0)
            {
                MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
                commonViewModel.MemberConatctInfo = contactInfoViewModel;
                commonViewModel.ProfilePercentage += 20;
            }

            Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(MemberId);
            if (member_education.MemberId > 0)
            {
                MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
                commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;
                commonViewModel.ProfilePercentage += 20;
            }

            List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(MemberId);
            List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
            foreach (Member_FamilyDetails family in member_Family)
            {
                memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
            }
            if(memberFamilies.Count>0)
            {
                commonViewModel.ProfilePercentage += 20;
            }

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(MemberId);
            if (member_Payments.MemberId > 0)
            {
                MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
                commonViewModel.MemberPaymentInfo = paymentViewModel;
                commonViewModel.ProfilePercentage += 20;
            }
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
        [HttpPost]
        public IActionResult AddToList(MemberFormCommonViewModel model)
        {
            if (Request.Method == HttpMethods.Post)
            {
                Member_FamilyDetails member_Family = mapper.Map<Member_FamilyDetails>(model.MemberFamilyInfo);
                if (model.MemberFamilyInfo.IsNew)
                    familyDetailsDataFactory.AddDetails(member_Family);
                else
                    familyDetailsDataFactory.UpdateDetails(member_Family);
            }
            ModelState.Clear();


            return RedirectToAction("ProfileUpdate");
        }
        public IActionResult MemberSearchDashBoard()
        {
            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                Banners = BannerHelper.GetBanners()
            };
            return View(viewModel);
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
            if (model.Pincode > 0)
                keyValuePairs.Add("Pincode", Convert.ToString(model.Pincode));

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
            MemberSearchByNativeViewModel viewModel = new MemberSearchByNativeViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult MemberSearchByNative(MemberSearchByNativeViewModel model)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(model.NativePlace))
                keyValuePairs.Add("NativePlace", model.NativePlace);
            if (!string.IsNullOrEmpty(model.NativePlaceTaluka))
                keyValuePairs.Add("NativePlaceTaluka", model.NativePlaceTaluka);
            if (!string.IsNullOrEmpty(model.NativePlaceDistrict))
                keyValuePairs.Add("NativePlaceDistrict", model.NativePlaceDistrict);


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

        public IActionResult MemberSearchByQualification()
        {
            MemberSearchByQualificationViewModel model = new MemberSearchByQualificationViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult MemberSearchByQualification(MemberSearchByQualificationViewModel model)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(model.Qualification))
                keyValuePairs.Add("Qualification", model.Qualification);
            if (!string.IsNullOrEmpty(model.Proffession))
                keyValuePairs.Add("Proffession", model.Proffession);



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

        public IActionResult MemberDoctors()
        {
            List<MemberSearchResponse> memberData = searchDataFactory.GetAllDoctors();
            List<MemberSearchResponseViewModel> filteredMember = new List<MemberSearchResponseViewModel>();
            foreach (MemberSearchResponse response in memberData)
            {
                MemberSearchResponseViewModel responseViewModel = mapper.Map<MemberSearchResponseViewModel>(response);
                filteredMember.Add(responseViewModel);
            }
            MemberDoctorViewModel model = new MemberDoctorViewModel()
            {
                MemberList = filteredMember
            };
            return View(model);
        }

        //[Route("/MemberBloodGroupSearch")]
        public IActionResult MemberBloodGroupSearch()
        {
            List<string> areas = personalDataFactory.GetDistinctAreas().OrderBy(x => x).ToList();
            List<string> cities = personalDataFactory.GetDistinctCities().OrderBy(x=>x).ToList();
            MemberBloodGroupSearchViewModel viewModel = new MemberBloodGroupSearchViewModel()
            {
                Areas = areas.Select(x=>new SelectListItem() { Value=x,Text=x}).ToList(),
                Cities = cities.Select(x => new SelectListItem() { Value = x, Text = x }).ToList()
            };
            viewModel.Areas.Insert(0, new SelectListItem() { Text = " Select Area ", Value = "0" });
            viewModel.Cities.Insert(0, new SelectListItem() { Text = " Select City ", Value = "0" });
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult MemberBloodGroupSearch(MemberBloodGroupSearchViewModel searchViewModel)
        {
            List<string> areas = personalDataFactory.GetDistinctAreas();
            List<string> cities = personalDataFactory.GetDistinctCities();

            searchViewModel.Areas = areas.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            searchViewModel.Cities = cities.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            searchViewModel.Areas.Insert(0, new SelectListItem() { Text = " Select Area ", Value = "0" });
            searchViewModel.Cities.Insert(0, new SelectListItem() { Text = " Select City ", Value = "0" });

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (searchViewModel.BloodGroup!="0")
                keyValuePairs.Add("BloodGroup", searchViewModel.BloodGroup);
            if (searchViewModel.Gender != "0")
                keyValuePairs.Add("Gender", searchViewModel.Gender);
            if (searchViewModel.Area != "0")
                keyValuePairs.Add("Area", searchViewModel.Area);
            if (searchViewModel.City != "0")
                keyValuePairs.Add("City", searchViewModel.City);
            if (!string.IsNullOrEmpty(searchViewModel.FirstName))
                keyValuePairs.Add("FirstName", searchViewModel.FirstName);
            if (!string.IsNullOrEmpty(searchViewModel.LastName))
                keyValuePairs.Add("LastName", searchViewModel.LastName);



            List<MemberSearchResponse> memberData = searchDataFactory.GetAllMemberssSearchResultByFilterValues(keyValuePairs);
            List<MemberSearchResponseViewModel> filteredMember = new List<MemberSearchResponseViewModel>();
            foreach (MemberSearchResponse response in memberData)
            {
                MemberSearchResponseViewModel responseViewModel = mapper.Map<MemberSearchResponseViewModel>(response);
                filteredMember.Add(responseViewModel);
            }
            searchViewModel.MemberList = filteredMember;
            return View(searchViewModel);
        }

        public IActionResult ViewMemberProfile(int MemberId)
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();

           // var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            //int MemberId = Convert.ToInt32(memberId);

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
    }
}
