using Microsoft.AspNetCore.Mvc;
using SBMMember.Web.Helper;
using SBMMember.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SBMMember.Data.DataFactory;
using SBMMember.Models;
using SBMMember.Data;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace SBMMember.Web.Controllers
{
    public class OTPVerifyController : Controller
    {
        private readonly IMemberDataFactory MemberDataFactory;
        private readonly IMemberPersonalDataFactory personalDataFactory;
        private readonly IMemberContactDetailsDataFactory contactDetailsDataFactory;
        private readonly IMemberEducationEmploymentDataFactory educationEmploymentDataFactory;
        private readonly IMemberFamilyDetailsDataFactory familyDetailsDataFactory;
        private readonly IMemberPaymentsDataFactory paymentsDataFactory;
        private readonly SBMMemberDBContext dBContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public OTPVerifyController(IMemberDataFactory dataFactory,
             IMemberPersonalDataFactory memberPersonalDataFactory,
            IMemberContactDetailsDataFactory memberContactDetailsDataFactory,
            IMemberEducationEmploymentDataFactory memberEducationEmploymentDataFactory,
            IMemberFamilyDetailsDataFactory memberFamilyDetailsDataFactory,
            IMemberPaymentsDataFactory _paymentsDataFactory,
            SBMMemberDBContext sBMMemberDBContext,
            IConfiguration _configuration,IMapper _mapper)
        {
            MemberDataFactory = dataFactory;
            personalDataFactory = memberPersonalDataFactory;
            contactDetailsDataFactory = memberContactDetailsDataFactory;
            educationEmploymentDataFactory = memberEducationEmploymentDataFactory;
            familyDetailsDataFactory = memberFamilyDetailsDataFactory;
            paymentsDataFactory = _paymentsDataFactory;
            dBContext = sBMMemberDBContext;
            configuration = _configuration;
            mapper = _mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendOTP(string mobile)
        {
            var member = MemberDataFactory.GetDetailsByMemberMobile(mobile);
            if (member.Mobile != null && member.MemberId > 0)
            {
                
               
                    ViewBag.AlreadyRegistered = $"{mobile} is already registered.Try with another number. OR Use below login option to submit remaining form details";
                    return View("Index");
                
            }
            else
            {
               
                string OTP = SMSHelper.GenerateOTP();
                string message = $"{OTP} is your OTP for login to Samata Bhratru Mandal (PCMC Pune) Vadhu Var Melava test.com registration portal. Validity for 30 minutes only. Please do not share to anyone else.";
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
                return View("VerifyOTP", loginViewModel);
            }
           
        }

        public IActionResult VerifyMPin(LoginViewModel _viewModel)
        {
            var member = MemberDataFactory.GetDetailsByMemberMobile(_viewModel.MobileNumber);
            if (member.MemberId>0 && member.Mobile.Trim()==_viewModel.MobileNumber.Trim() && member.Mpin.Trim()==_viewModel.MPIN.Trim())
            {
                Member_PersonalDetails personalDetails = personalDataFactory.GetMemberPersonalDetailsByMemberId(member.MemberId);
                Member_ContactDetails contactDetails = contactDetailsDataFactory.GetDetailsByMemberId(member.MemberId);
                Member_EducationEmploymentDetails educationEmploymentDetails = educationEmploymentDataFactory.GetDetailsByMemberId(member.MemberId);
                List<Member_FamilyDetails> familyDetails = familyDetailsDataFactory.GetFamilyDetailsByMemberId(member.MemberId);
                Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(member.MemberId);
                if (personalDetails == null || personalDetails.MemberId == 0)
                {
                    MemberPerosnalInfoViewModel perosnalInfoViewModel = new MemberPerosnalInfoViewModel()
                    {
                        MemberId = member.MemberId
                    };
                    return RedirectToAction("MemberPersonalInfo", "Member", new { MemberId = member.MemberId });
                }
                else if (contactDetails == null || contactDetails.MemberId == 0)
                {
                    MemberContactInfoViewModel viewModel = new MemberContactInfoViewModel()
                    {
                        MemberId = member.MemberId
                    };
                    return RedirectToAction("MemberContactInfo", "Member", new { MemberId = member.MemberId });
                }
                else if (educationEmploymentDetails == null || educationEmploymentDetails.MemberId == 0)
                {
                    MemberEducationEmploymentInfoViewModel viewModel = new MemberEducationEmploymentInfoViewModel()
                    {
                        MemberId = member.MemberId
                    };
                    return RedirectToAction("MemberEduEmpInfo", "Member", new { MemberId = member.MemberId });
                }
                else if (familyDetails == null || familyDetails.Count < 1)
                {
                    MemberFamilyInfoViewModel viewModel = new MemberFamilyInfoViewModel()
                    {
                        MemberId = member.MemberId
                    };
                    return RedirectToAction("MemberFamilyInfo", "Member", new { MemberId = member.MemberId });
                }
                else if (member_Payments == null || member_Payments.MemberId == 0)
                {
                    var data = (from memberbasic in dBContext.Member_PersonalDetails
                                join memberContact in dBContext.Member_ContactDetails
                                 on memberbasic.MemberId equals memberContact.MemberId
                                where memberContact.MemberId == member.MemberId
                                select new
                                {
                                    Name = $"{memberbasic.FirstName} {memberbasic.MiddleName} {memberbasic.LastName}",
                                    Conatct = memberContact.Mobile1,
                                    Email = memberContact.EmailId
                                }
                          ).FirstOrDefault();
                    MemberPaymentViewModel memberPayment = new MemberPaymentViewModel()
                    {
                        MemberId = member.MemberId,
                        MemberName = data.Name,
                        Mobile = data.Conatct,
                        Email = data.Email,
                        Amount = Convert.ToInt32(configuration.GetSection("SubscriptionCharges").Value)
                    };

                    return RedirectToAction("AcceptMemberPayment", "Payment", memberPayment);
                }
                else
                {
                    ViewBag.VerifyMPinMessage = $"{_viewModel.MobileNumber} is already registered.Try with another number. OR If you recived your profile activation message please navigate to login page.";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.VerifyMPinMessage = $"Mobile No or Mpin invalid. Try with correct mobile and Mpin ";
                return View("Index");
            }
            
        }
        public IActionResult VerifyMPinNew(LoginViewModel _viewModel)
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();

            var member = MemberDataFactory.GetDetailsByMemberMobile(_viewModel.MobileNumber);
            if (member.MemberId > 0 && member.Mobile.Trim() == _viewModel.MobileNumber.Trim() && member.Mpin.Trim() == _viewModel.MPIN.Trim())
            {
                Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(member.MemberId);
                MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
               
                commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;

                Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(member.MemberId);
                MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
               
                commonViewModel.MemberConatctInfo = contactInfoViewModel;

                Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(member.MemberId);
                MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
               

                commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;

                List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(member.MemberId);
                List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
                foreach (Member_FamilyDetails family in member_Family)
                {
                    memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
                }
               
                ViewBag.MemberList = memberFamilies;

                Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(member.MemberId);
                MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
               
                commonViewModel.MemberPaymentInfo = paymentViewModel;

                if (perosnalInfoViewModel.MemberId == 0)
                    perosnalInfoViewModel.ActiveTab = "Checked";
                else if (contactInfoViewModel.MemberId == 0)
                    contactInfoViewModel.ActiveTab = "Checked";
                else if (educationEmploymentInfoViewModel.MemberId == 0)
                    educationEmploymentInfoViewModel.ActiveTab = "Checked";
                else if (memberFamilies.Count == 0)
                    ViewBag.FamilyTab = "Checked";
                else if (paymentViewModel.MemberId == 0)
                    paymentViewModel.ActiveTab = "Checked";

                return View("MemberRegistration", commonViewModel);
               
            }
            else
            {
                ViewBag.VerifyMPinMessage = $"Mobile No or Mpin invalid. Try with correct mobile and Mpin ";
                return View("Index");
            }

        }
        //[HttpPost]
        //public IActionResult MemberPersonalInfo(MemberFormCommonViewModel viewModel)
        //{

        //    Member_PersonalDetails personalDetails = mapper.Map<Member_PersonalDetails>(model);
        //    //personalDetails.CreateDate = DateTime.Now;
        //    memberDataFactory.UpdateName(personalDetails.MemberId, personalDetails.FirstName, personalDetails.MiddleName, personalDetails.LastName);

        //    var response = personalDataFactory.AddMemberPersonalDetails(personalDetails);
        //    if (response.Result == "Success")
        //    {
        //        MemberContactInfoViewModel viewModel = new MemberContactInfoViewModel()
        //        {
        //            MemberId = model.MemberId
        //        };
        //        return View("MemberContactInfo", viewModel);
        //    }
        //    else
        //        return View();

        //}
        public IActionResult VerifyOTP()
        {

            return View();
        }
        [HttpPost]
        public IActionResult VerifyOTP(LoginViewModel model)
        {
            if (model.SentOTP == model.OTPInput)
                model.IsOTPVerified = true;
            else
                model.IsOTPMismatch = true;

            return View("VerifyOTP", model);
        }

        [HttpPost]
        public IActionResult RedirectToSetMPIN(LoginViewModel model)
        {
            HttpContext.Session.SetString("Mobile", model.MobileNumber);
            return RedirectToAction("SetPasswordAndPin", "Member", model);
        }

    }
}
