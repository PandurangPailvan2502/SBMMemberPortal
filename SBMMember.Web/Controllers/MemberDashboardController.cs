﻿using AutoMapper;
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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NToastNotify;
using Newtonsoft.Json;
using System.Web;
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
        private readonly IMarqueeDataFactory marqueeDataFactory;
        private IWebHostEnvironment Environment;
        private readonly IJobPostingDataFactory jobPostingDataFactory;
        private readonly IToastNotification _toastNotification;
        private readonly IMemberMeetingsDataFactory meetingsDataFactory;
        private readonly IBannerAdsDataFactory bannerAdsDataFactory;
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
            IConfiguration _configuration,
            IMarqueeDataFactory _marqueeDataFactory,
            IWebHostEnvironment _environment,
            IJobPostingDataFactory _jobPostingDataFactory, IToastNotification toast,
            IMemberMeetingsDataFactory memberMeetingsDataFactory,
            IBannerAdsDataFactory adsDataFactory)
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
            marqueeDataFactory = _marqueeDataFactory;
            Environment = _environment;
            jobPostingDataFactory = _jobPostingDataFactory;
            _toastNotification = toast;
            meetingsDataFactory = memberMeetingsDataFactory;
            bannerAdsDataFactory = adsDataFactory;
        }

        [Authorize]
        public IActionResult Index()
        {

            List<string> marquess = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();
            string marqueeText = string.Join(",", marquess);

            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                //Banners = BannerHelper.GetBanners(),
                MarqueeString = marqueeText,
                NotificationCount = 0,
                NewMemberCount = searchDataFactory.GetRecentMembersCount(),
                RecentJobCount = jobPostingDataFactory.RecentJobCount(),
                MemberMeeting = meetingsDataFactory.GetMemberMeetings().Where(x => x.Status == "Active" && x.IsActive == 1).FirstOrDefault()

            };
            return View(viewModel);
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult UploadProfileImage(MemberFormCommonViewModel commonViewModel)
        {
            string newFileName = "";

            string path = Path.Combine(this.Environment.WebRootPath, "MemberProfileImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string extension = Path.GetExtension(commonViewModel.file.FileName);
            newFileName = "MemberId" + commonViewModel.MemberId + extension;
            string FilePath = $"~/MemberProfileImages/{newFileName}";

            string fullPath = Path.Combine(path, newFileName);


            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                commonViewModel.file.CopyTo(stream);

            }
            ResponseDTO response = personalDataFactory.UpdateMemberProfileImage(commonViewModel.MemberId, FilePath);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);

            return RedirectToAction("ProfileUpdate");
        }
        public IActionResult ProfileUpdate()
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();

            var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            int MemberId = Convert.ToInt32(memberId);
            commonViewModel.ProfilePercentage = 0;
            commonViewModel.MemberId = MemberId;


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
                // contactInfoViewModel.ActiveTab = TempData["ContactTabActive"].ToString();
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
            familyInfoview.IsNew = true;
            //familyInfoview.DOB = familyInfoview.DOB == DateTime.MinValue ? DateTime.Now.AddYears(-72) : familyInfoview.DOB;
            commonViewModel.MemberFamilyInfo = familyInfoview;
            if (memberFamilies.Count > 0)
                commonViewModel.ProfilePercentage += 20;

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(MemberId);
            if (member_Payments.MemberId > 0)
            {
                MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
                commonViewModel.MemberPaymentInfo = paymentViewModel;
                commonViewModel.ProfilePercentage += 20;
            }
            if (TempData.ContainsKey("PersonalTabActive"))
            {
                commonViewModel.MemberPersonalInfo.ActiveTab = "Checked";
                TempData.Remove("PersonalTabActive");
            }
            else if (TempData.ContainsKey("ContactTabActive"))
            {
                commonViewModel.MemberConatctInfo.ActiveTab = "Checked";
                TempData.Remove("ContactTabActive");
            }
            else if (TempData.ContainsKey("EduEmpTabActive"))
            {
                commonViewModel.MemberEducationEmploymentInfo.ActiveTab = "Checked";
                TempData.Remove("EduEmpTabActive");
            }
            else if (TempData.ContainsKey("FamilyTabActive"))
            {
                commonViewModel.MemberFamilyInfo.ActiveTab = "Checked";
                TempData.Remove("FamilyTabActive");
            }
            else
            {
                commonViewModel.MemberPersonalInfo.ActiveTab = "Checked";
            }

            ViewBag.Message = TempData["Message"];
            return View(commonViewModel);
        }


        public IActionResult ProfileUpdateForFamily(MemberFamilyInfoViewModel memberFamily)
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
            //MemberFamilyInfoViewModel familyInfoview = new MemberFamilyInfoViewModel();
            memberFamily.MemberFamilyDetails = memberFamilies;
            memberFamily.MemberId = MemberId;
            memberFamily.IsNew = false;
            commonViewModel.MemberFamilyInfo = memberFamily;
            if (memberFamilies.Count > 0)
                commonViewModel.ProfilePercentage += 20;

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(MemberId);
            if (member_Payments.MemberId > 0)
            {
                MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
                commonViewModel.MemberPaymentInfo = paymentViewModel;
                commonViewModel.ProfilePercentage += 20;
            }

            return View("ProfileUpdate", commonViewModel);
        }
        public IActionResult EditFamilyMember(int id)
        {
            Member_FamilyDetails member_Family = familyDetailsDataFactory.GetDetailsByMemberId(id);
            MemberFamilyInfoViewModel model = mapper.Map<MemberFamilyInfoViewModel>(member_Family);

            return RedirectToAction("ProfileUpdateForFamily", model);
        }
        public IActionResult DeleteFamilyMember(int id, int memberId)
        {
            familyDetailsDataFactory.DeleteById(id);
            _toastNotification.AddSuccessToastMessage("Family member removed successfully.");
            return RedirectToAction("ProfileUpdate");
        }
        public IActionResult ViewLoggedMemberProfile()
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
            if (memberFamilies.Count > 0)
            {
                commonViewModel.ProfilePercentage += 20;
            }
            MemberFamilyInfoViewModel memberFamilyInfo = new MemberFamilyInfoViewModel();
            memberFamilyInfo.MemberFamilyDetails = memberFamilies;
            commonViewModel.MemberFamilyInfo = memberFamilyInfo;

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(MemberId);
            if (member_Payments.MemberId > 0)
            {
                MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
                commonViewModel.MemberPaymentInfo = paymentViewModel;
                commonViewModel.ProfilePercentage += 20;
            }
            return View(commonViewModel);
        }
        [HttpGet]
        public JsonResult GetFmemberDetails(int id)
        {

            var familyMemberInfo = familyDetailsDataFactory.GetDetailsByMemberId(id);

            return Json(familyMemberInfo);
        }
        [HttpPost]
        public IActionResult MemberPersonalInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_PersonalDetails member_Personal = mapper.Map<Member_PersonalDetails>(formCommonViewModel.MemberPersonalInfo);
            ResponseDTO response = personalDataFactory.UpdateMemberPersonalDetailsForProfileUpdate(member_Personal);
            TempData["PersonalTabActive"] = "Checked";
            TempData["Message"] = "Personal details updated successfully.";
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("ProfileUpdate");
        }
        [HttpPost]
        public IActionResult MemberContactInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_ContactDetails member_Contact = mapper.Map<Member_ContactDetails>(formCommonViewModel.MemberConatctInfo);
            Members member = new Members()
            {
                MemberId = formCommonViewModel.MemberConatctInfo.MemberId,
                Mobile = formCommonViewModel.MemberConatctInfo.Mobile1
            };
            memberDataFactory.UpdateMobileNo(member);
            TempData["ContactTabActive"] = "Checked";
            TempData["Message"] = "Contact details updated successfully.";
            ResponseDTO response = contactDetailsDataFactory.UpdateDetails(member_Contact);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);

            return RedirectToAction("ProfileUpdate");
        }
        [HttpPost]
        public IActionResult SendOTP(string mobile)
        {
            var member = memberDataFactory.GetDetailsByMemberMobile(mobile);


            string OTP = SMSHelper.GenerateOTP();
            //string message = $"{OTP} is your OTP for mobile change request. OTP valid for 5 min only. If you have not generated this request, please contact Samata Bhratru Mandal (PCMC Pune) on toll free 02071173733.";
            string message = $"{OTP} is your OTP for mobile change request. Valid for 5 min only. If you have not generated this request, please contact Samata Bhratru Mandal (PCMC Pune) on toll free 02071173733.";
            SMSHelper.SendSMS(mobile, message);
            string maskedMobile = mobile.Substring(mobile.Length - 4).PadLeft(mobile.Length, 'x');
            ViewBag.MobileNumber = mobile;
            ViewBag.MaskedMobile = maskedMobile;
            ViewBag.SentOTP = OTP;
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                MobileNumber = mobile,
                MaskedMobileNumber = maskedMobile,
                SentOTP = OTP
            };
            return Json(OTP);


        }
        [HttpPost]
        public IActionResult MemberEduEmpInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_EducationEmploymentDetails member_Education = mapper.Map<Member_EducationEmploymentDetails>(formCommonViewModel.MemberEducationEmploymentInfo);
            ResponseDTO response = educationEmploymentDataFactory.UpdateDetails(member_Education);
            TempData["EduEmpTabActive"] = "Checked";
            TempData["Message"] = "Education and Employment details updated successfully.";
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("ProfileUpdate");
        }
        [HttpPost]
        public IActionResult AddToList(MemberFormCommonViewModel model)
        {
            if (Request.Method == HttpMethods.Post)
            {
                ResponseDTO response = new ResponseDTO();
                Member_FamilyDetails member_Family = mapper.Map<Member_FamilyDetails>(model.MemberFamilyInfo);
                if (model.MemberFamilyInfo.IsNew)
                    response = familyDetailsDataFactory.AddDetails(member_Family);
                else
                    response = familyDetailsDataFactory.UpdateDetails(member_Family);
                TempData["Message"] = "Family Member details Added/updated successfully.";
                TempData["FamilyTabActive"] = "Checked";
                if (response.Result == "Success")
                    _toastNotification.AddSuccessToastMessage(response.Message);
                else
                    _toastNotification.AddErrorToastMessage(response.Message);
            }
            ModelState.Clear();


            return RedirectToAction("ProfileUpdate");
        }
        public IActionResult MemberSearchDashBoard()
        {
            List<BannerClass> bannerClasses = bannerAdsDataFactory.GetBannerAds().Select(x => new BannerClass() { heading = x.Heading, imageURL = x.ImageURL }).ToList();

            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                //Banners = BannerHelper.GetBanners()
                Banners = bannerClasses
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
            if (!string.IsNullOrEmpty(model.MemberId))
                keyValuePairs.Add("MemberId", model.MemberId);
            List<MemberSearchResponse> memberData = searchDataFactory.GetAllMemberssSearchResultByFilterValues(keyValuePairs);
            List<MemberSearchResponseViewModel> filteredMember = new List<MemberSearchResponseViewModel>();
            foreach (MemberSearchResponse response in memberData)
            {
                MemberSearchResponseViewModel responseViewModel = mapper.Map<MemberSearchResponseViewModel>(response);
                filteredMember.Add(responseViewModel);
            }
            model.MemberList = filteredMember;
            if (filteredMember.Count > 0)
            {
                _toastNotification.AddInfoToastMessage($"{filteredMember.Count} matching results found..");
            }
            else
            {
                _toastNotification.AddWarningToastMessage($"No matching results found..");
            }
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
            if (filteredMember.Count > 0)
            {
                _toastNotification.AddInfoToastMessage($"{filteredMember.Count} matching results found..");
            }
            else
            {
                _toastNotification.AddWarningToastMessage($"No matching results found..");
            }
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
            if (filteredMember.Count > 0)
            {
                _toastNotification.AddInfoToastMessage($"{filteredMember.Count} matching results found..");
            }
            else
            {
                _toastNotification.AddWarningToastMessage($"No matching results found..");
            }
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
            if (filteredMember.Count > 0)
            {
                _toastNotification.AddInfoToastMessage($"{filteredMember.Count} matching results found..");
            }
            else
            {
                _toastNotification.AddWarningToastMessage($"No matching results found..");
            }
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
            _toastNotification.AddInfoToastMessage($"{filteredMember.Count} matching result(s) found..");
            return View(model);
        }

        //[Route("/MemberBloodGroupSearch")]
        public IActionResult MemberBloodGroupSearch()
        {
            List<string> areas = personalDataFactory.GetDistinctAreas().OrderBy(x => x).ToList();
            List<string> cities = personalDataFactory.GetDistinctCities().OrderBy(x => x).ToList();
            MemberBloodGroupSearchViewModel viewModel = new MemberBloodGroupSearchViewModel()
            {
                Areas = areas.Select(x => new SelectListItem() { Value = x, Text = x }).ToList(),
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
            if (searchViewModel.BloodGroup != "0")
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
            if (filteredMember.Count > 0)
            {
                _toastNotification.AddInfoToastMessage($"{filteredMember.Count} matching results found..");
            }
            else
            {
                _toastNotification.AddWarningToastMessage($"No matching results found..");
            }
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

        public IActionResult ViewAndDownloadMemberCard(string id)
        {
            int memberId = Convert.ToInt32(id);
            var memberPersonalInfo = personalDataFactory.GetMemberPersonalDetailsByMemberId(memberId);
            var paymentInfo = paymentsDataFactory.GetDetailsByMemberId(memberId);
            MemberCardViewModel memberCardViewModel = new MemberCardViewModel()
            {
                MemberId = memberId,
                MemberName = $"{memberPersonalInfo.FirstName} {memberPersonalInfo.MiddleName} {memberPersonalInfo.LastName}",
                MemberProfileImage = memberPersonalInfo.MemberProfileImage,
                MembershipId = paymentInfo.MembershipId
            };
            return View(memberCardViewModel);
        }

        [HttpPost]
        public IActionResult ViewAndDownloadMemberCard(MemberCardViewModel model)
        {


            return View();
        }
    }
}
