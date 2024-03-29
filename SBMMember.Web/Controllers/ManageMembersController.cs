﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SBMMember.Data.DataFactory;
using SBMMember.Web.Models;
using SBMMember.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using SBMMember.Web.Helper;
using NToastNotify;
using System.IO;
using Microsoft.AspNetCore.Hosting;
namespace SBMMember.Web.Controllers
{
    // [Authorize]
    public class ManageMembersController : Controller
    {
        private readonly IMemberDataFactory memberDataFactory;
        private readonly IMemberSearchDataFactory memberSearchDataFactory;

        private readonly IMemberPersonalDataFactory personalDataFactory;
        private readonly IMemberContactDetailsDataFactory contactDetailsDataFactory;
        private readonly IMemberEducationEmploymentDataFactory educationEmploymentDataFactory;
        private readonly IMemberFamilyDetailsDataFactory familyDetailsDataFactory;
        private readonly IMemberPaymentsDataFactory paymentsDataFactory;
        private readonly IMapper mapper;
        private readonly IMemberFormStatusDataFactory formStatusDataFactory;
        private readonly IEventTitlesDataFactory eventTitlesDataFactory;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment Environment;

        public ManageMembersController(IMemberDataFactory dataFactory,
            IMemberSearchDataFactory searchDataFactory,
            IMemberPersonalDataFactory _personalDataFactory,
            IMemberContactDetailsDataFactory _contactDetailsDataFactory,
            IMemberEducationEmploymentDataFactory _educationEmploymentDataFactory,
            IMemberFamilyDetailsDataFactory _familyDetailsDataFactory,
            IMemberPaymentsDataFactory _paymentsDataFactory,
            IMapper _mapper,
            IMemberFormStatusDataFactory _formStatusDataFactory,
            IEventTitlesDataFactory _eventTitlesDataFactory,
            IToastNotification toast,
            IWebHostEnvironment environment
            )
        {
            memberDataFactory = dataFactory;
            memberSearchDataFactory = searchDataFactory;
            personalDataFactory = _personalDataFactory;
            contactDetailsDataFactory = _contactDetailsDataFactory;
            educationEmploymentDataFactory = _educationEmploymentDataFactory;
            familyDetailsDataFactory = _familyDetailsDataFactory;
            paymentsDataFactory = _paymentsDataFactory;
            mapper = _mapper;
            formStatusDataFactory = _formStatusDataFactory;
            eventTitlesDataFactory = _eventTitlesDataFactory;
            _toastNotification = toast;
            Environment = environment;
        }
        public IActionResult MemberList()
        {
            MemberListViewModel model = new MemberListViewModel()
            {
                memberList = memberSearchDataFactory.GetAllApprovedMembers()
            };
            return View(model);
        }
        public IActionResult NewMemberList()
        {
            MemberListViewModel model = new MemberListViewModel()
            {
                memberList = memberSearchDataFactory.GetAllNewlySubmittedMembers()
            };
            return View(model);
        }
        public IActionResult VerifyMemberProfile(int MemberId)
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();

            //var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            int _MemberId = MemberId;
            commonViewModel.MemberId = MemberId;
            Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(_MemberId);
            MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
            commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;

            Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(_MemberId);
            MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
            commonViewModel.MemberConatctInfo = contactInfoViewModel;

            Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(_MemberId);
            MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
            commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;

            List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(_MemberId);
            List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
            foreach (Member_FamilyDetails family in member_Family)
            {
                memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
            }
            MemberFamilyInfoViewModel familyInfoViewModel = new MemberFamilyInfoViewModel();
            familyInfoViewModel.MemberFamilyDetails = memberFamilies;
            //familyInfoViewModel.DOB = DateTime.Now.AddYears(-72);
            commonViewModel.MemberFamilyInfo = familyInfoViewModel;

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(_MemberId);
            MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
            paymentViewModel.LastMemberShipId = paymentsDataFactory.LastMembershipNo();
            commonViewModel.MemberPaymentInfo = paymentViewModel;

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
            else if(TempData.ContainsKey("PaymentTabActive"))
            {
                commonViewModel.MemberPaymentInfo.ActiveTab = "Checked";
                TempData.Remove("PaymentTabActive");
            }
            else
            {
                commonViewModel.MemberPersonalInfo.ActiveTab = "Checked";
            }

            return View(commonViewModel);
            //return  RedirectToAction("NewMemberList");
        }
        public IActionResult VerifyMemberProfileNew(MemberFamilyInfoViewModel familymodel)
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();

            //var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            int _MemberId = familymodel.MemberId;

            Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(_MemberId);
            MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
            commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;

            Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(_MemberId);
            MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
            commonViewModel.MemberConatctInfo = contactInfoViewModel;

            Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(_MemberId);
            MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
            commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;

            List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(_MemberId);
            List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
            foreach (Member_FamilyDetails family in member_Family)
            {
                memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
            }
            //MemberFamilyInfoViewModel familyInfoViewModel = new MemberFamilyInfoViewModel();
            //familyInfoViewModel.MemberFamilyDetails = memberFamilies;
            //commonViewModel.MemberFamilyInfo = familyInfoViewModel;
            familymodel.MemberFamilyDetails = memberFamilies;
            familymodel.IsNew = false;
            commonViewModel.MemberFamilyInfo = familymodel;
            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(_MemberId);
            MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
            paymentViewModel.LastMemberShipId = paymentsDataFactory.LastMembershipNo();
            commonViewModel.MemberPaymentInfo = paymentViewModel;


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
            else if (TempData.ContainsKey("PaymentTabActive"))
            {
                commonViewModel.MemberPaymentInfo.ActiveTab = "Checked";
                TempData.Remove("PaymentTabActive");
            }
            else
            {
                commonViewModel.MemberPersonalInfo.ActiveTab = "Checked";
            }


            return View("VerifyMemberProfile", commonViewModel);
            //return  RedirectToAction("NewMemberList");
           // return RedirectToAction("VerifyMemberProfile", new { MemberId = commonViewModel.MemberFamilyInfo.MemberId });
        }

       
        public IActionResult EditFamilyMember(int id)
        {
            Member_FamilyDetails member_Family = familyDetailsDataFactory.GetDetailsByMemberId(id);
            MemberFamilyInfoViewModel model = mapper.Map<MemberFamilyInfoViewModel>(member_Family);
            TempData["FamilyTabActive"] = "Checked";
            return RedirectToAction("VerifyMemberProfileNew", model);
        }
        public IActionResult DeleteFamilyMember(int id, int memberId)
        {
            familyDetailsDataFactory.DeleteById(id);
            _toastNotification.AddSuccessToastMessage("Family member removed successfully.");
            return RedirectToAction("VerifyMemberProfile", new { MemberId = memberId });
        }
        public IActionResult AddToList(MemberFormCommonViewModel commonViewModel)
        {
            Member_FamilyDetails member_Family = mapper.Map<Member_FamilyDetails>(commonViewModel.MemberFamilyInfo);
            ResponseDTO response = new ResponseDTO();
            if (commonViewModel.MemberFamilyInfo.IsNew)
                response = familyDetailsDataFactory.AddDetails(member_Family);
            else
                response = familyDetailsDataFactory.UpdateDetailsNoTranslation(member_Family);

            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            TempData["FamilyTabActive"] = "Checked";
            return RedirectToAction("VerifyMemberProfile", new { MemberId = commonViewModel.MemberFamilyInfo.MemberId });
        }
        //public IActionResult ApproveMemberProfile(int memberId)
        //{
        //    return RedirectToAction("NewMemberList");
        //}
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
            return RedirectToAction("VerifyMemberProfile", new { MemberId = commonViewModel.MemberId });
        }
        [HttpPost]
        public IActionResult MemberPersonalInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_PersonalDetails member_Personal = mapper.Map<Member_PersonalDetails>(formCommonViewModel.MemberPersonalInfo);
            ResponseDTO response = personalDataFactory.UpdateMemberPersonalDetailsNoTranslation(member_Personal);
            TempData["PersonalTabActive"] = "Checked";
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("VerifyMemberProfile", new { MemberId = formCommonViewModel.MemberPersonalInfo.MemberId });
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
            ResponseDTO response = contactDetailsDataFactory.UpdateDetailsNoTransalation(member_Contact);
            TempData["ContactTabActive"] = "Checked";
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("VerifyMemberProfile", new { MemberId = formCommonViewModel.MemberConatctInfo.MemberId });
        }

        [HttpPost]
        public IActionResult MemberEduEmpInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_EducationEmploymentDetails member_Education = mapper.Map<Member_EducationEmploymentDetails>(formCommonViewModel.MemberEducationEmploymentInfo);
            ResponseDTO response = educationEmploymentDataFactory.UpdateDetailsNoTranslation(member_Education);
            TempData["EduEmpTabActive"] = "Checked";
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("VerifyMemberProfile", new { MemberId = formCommonViewModel.MemberEducationEmploymentInfo.MemberId });
        }
        [HttpPost]
        public IActionResult MemberPaymentInfo(MemberFormCommonViewModel memberFormCommon)
        {
            Member_PaymentsAndReciepts member_Payments = mapper.Map<Member_PaymentsAndReciepts>(memberFormCommon.MemberPaymentInfo);
            ResponseDTO response = paymentsDataFactory.UpdateDetails(member_Payments);
            TempData["PaymentTabActive"] = "Checked";
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("VerifyMemberProfile", new { MemberId = memberFormCommon.MemberPaymentInfo.MemberId });
        }
        [HttpPost]
        public IActionResult MarkVerify(MemberFormCommonViewModel model)
        {
            Member_FormStatus formStatus = formStatusDataFactory.GetDetailsByMemberId(model.MemberId);
            formStatus.VerifiedBy = User.Identity.Name;
            formStatus.VerifiedDate = DateTime.Now;
            formStatus.FormStatus = "Verified";
            formStatusDataFactory.UpdateDetails(formStatus);
            //return RedirectToAction("VerifyMemberProfile", new { MemberId = model.MemberId });
            _toastNotification.AddSuccessToastMessage("Member successfully verified.");
            return RedirectToAction("NewMemberList");
        }

        //[HttpPost]
        public IActionResult ApproveMemberProfile(int memberId)
        {
            Member_FormStatus formStatus = formStatusDataFactory.GetDetailsByMemberId(memberId);
            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(memberId);
            Members member = memberDataFactory.GetDetailsByMemberId(memberId);
            string memberName = $"{member.FirstName} {member.LastName}";
            //string smstemp = $"Dear {memberName}, your membership is approved %26 activated by our team. {member_Payments.MembershipId} is your membership number. You can login to our official android app %26 enjoy the exclusive features by Samata Bhratru Mandal (PCMC Pune). Toll Free 02071173733.";
            string smstemp = $"Dear {memberName}, your membership is approved %26 activated by our team. Your membership number is {member_Payments.MembershipId}. You may please log in to our official Android app %26 enjoy the exclusive features of Samata Bhratru Mandal (PCMC Pune). Toll-Free 02071173733.";
            if (member != null && member.MemberId > 0)
            {
                SMSHelper.SendSMS(member.Mobile, smstemp);
            }
            formStatus.ApprovedBy = User.Identity.Name;
            formStatus.ApprovedDate = DateTime.Now;
            formStatus.FormStatus = "Approved";
            formStatusDataFactory.UpdateDetails(formStatus);
            //return RedirectToAction("VerifyMemberProfile", new { MemberId = model.MemberId });
            _toastNotification.AddSuccessToastMessage("Membership approved successfully.");
            return RedirectToAction("NewMemberList");
        }

        public IActionResult DeActivateMemberProfile(int memberId)
        {
            Members member = memberDataFactory.GetDetailsByMemberId(memberId);
            string memberName = $"{member.FirstName} {member.LastName}";
            Member_FormStatus formStatus = formStatusDataFactory.GetDetailsByMemberId(memberId);
            formStatus.FormStatus = "DeActivated";
            formStatus.AdminComments = "Member profile DeActivated successfully.";
           ResponseDTO response= formStatusDataFactory.UpdateDetails(formStatus);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage($"Member profile for:{memberName} DeActivated successfully.");
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("VerifyMemberProfile", new { MemberId = memberId });
        }

    }
}
