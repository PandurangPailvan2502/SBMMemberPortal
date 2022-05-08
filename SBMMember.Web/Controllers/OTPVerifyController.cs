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
using System.IO;
using Microsoft.AspNetCore.Hosting;

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
        private readonly IMarqueeDataFactory marqueeDataFactory;
        private IWebHostEnvironment Environment;
        //private static List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();

        public OTPVerifyController(IMemberDataFactory dataFactory,
             IMemberPersonalDataFactory memberPersonalDataFactory,
            IMemberContactDetailsDataFactory memberContactDetailsDataFactory,
            IMemberEducationEmploymentDataFactory memberEducationEmploymentDataFactory,
            IMemberFamilyDetailsDataFactory memberFamilyDetailsDataFactory,
            IMemberPaymentsDataFactory _paymentsDataFactory,
            SBMMemberDBContext sBMMemberDBContext,
            IConfiguration _configuration, IMapper _mapper, IMarqueeDataFactory _marqueeDataFactory, IWebHostEnvironment _Environment)
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
            marqueeDataFactory = _marqueeDataFactory;
            Environment = _Environment;
        }
        public IActionResult Index()
        {
            List<string> marqueetxt = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();

            LoginViewModel model = new LoginViewModel()
            {
                MarqueeString = string.Join(", ", marqueetxt)
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SendOTP(string mobile)
        {
            var member = MemberDataFactory.GetDetailsByMemberMobile(mobile);
            if (member.Mobile != null && member.MemberId > 0)
            {

                List<string> marqueetxt = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();

                LoginViewModel model = new LoginViewModel()
                {
                    MarqueeString = string.Join(", ", marqueetxt)
                };
                ViewBag.AlreadyRegistered = $"{mobile} is already registered.Try with another number. OR Use below login option to submit remaining form details";
                return View("Index",model);

            }
            else
            {

                string OTP = SMSHelper.GenerateOTP();
                string message = $"{OTP} is your OTP for login to Samata Bhratru Mandal (PCMC Pune) member portal. OTP valid for 5 min only. Please do not share your OTP with anyone.";
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
            if (member.MemberId > 0 && member.Mobile.Trim() == _viewModel.MobileNumber.Trim() && member.Mpin.Trim() == _viewModel.MPIN.Trim())
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
            commonViewModel.ProfilePercentage = 0;
            var member = MemberDataFactory.GetDetailsByMemberMobile(_viewModel.MobileNumber);
            if (member.MemberId > 0 && member.Mobile.Trim() == _viewModel.MobileNumber.Trim() && member.Mpin.Trim() == _viewModel.MPIN.Trim())
            {
                commonViewModel.MemberId = member.MemberId;
                Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(member.MemberId);
                MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
                if (member_Personal.MemberId > 0)
                {
                    perosnalInfoViewModel.IsNew = false;
                    commonViewModel.ProfilePercentage += 20;
                }
                else
                    perosnalInfoViewModel.IsNew = true;
                perosnalInfoViewModel.MemberId = member.MemberId;
                perosnalInfoViewModel.BirthDate = perosnalInfoViewModel.BirthDate==DateTime.MinValue? DateTime.Now.AddYears(-72) : perosnalInfoViewModel.BirthDate;
                commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;

                Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(member.MemberId);
                MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
                if (member_contact.MemberId > 0)
                {
                    contactInfoViewModel.IsNew = false;
                    commonViewModel.ProfilePercentage += 20;
                }
                else
                    contactInfoViewModel.IsNew = true;
                contactInfoViewModel.MemberId = member.MemberId;
                commonViewModel.MemberConatctInfo = contactInfoViewModel;

                Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(member.MemberId);
                MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
                educationEmploymentInfoViewModel.MemberId = member.MemberId;
                if (member_education.MemberId > 0)
                {
                    educationEmploymentInfoViewModel.IsNew = false;
                    commonViewModel.ProfilePercentage += 20;
                }
                else
                    educationEmploymentInfoViewModel.IsNew = true;

                commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;

                List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(member.MemberId);
               List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
                foreach (Member_FamilyDetails family in member_Family)
                {
                    memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
                }
                MemberFamilyInfoViewModel familyInfoViewModel = new MemberFamilyInfoViewModel();
                familyInfoViewModel.MemberId = member.MemberId;
                familyInfoViewModel.DOB = familyInfoViewModel.DOB== DateTime.MinValue ? DateTime.Now.AddYears(-72):familyInfoViewModel.DOB;
                familyInfoViewModel.IsNew = true;
                familyInfoViewModel.MemberFamilyDetails = memberFamilies;
                if(memberFamilies.Count>0)
                    commonViewModel.ProfilePercentage += 20;
                commonViewModel.MemberFamilyInfo = familyInfoViewModel;
                //ViewBag.MemberList = memberFamilies;

                Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(member.MemberId);
                MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
                paymentViewModel.MemberId = member.MemberId;
                commonViewModel.MemberPaymentInfo = paymentViewModel;

                if (member_Personal.MemberId == 0)
                    perosnalInfoViewModel.ActiveTab = "Checked";
                else if (member_contact.MemberId == 0)
                    contactInfoViewModel.ActiveTab = "Checked";
                else if (member_education.MemberId == 0)
                    educationEmploymentInfoViewModel.ActiveTab = "Checked";
                else if ((memberFamilies.Count == 0 || member_Payments.MemberId > 0)&& member_Payments.MemberId == 0)
                    ViewBag.FamilyTab = "Checked";
                //else if (member_Payments.MemberId == 0)
                //    paymentViewModel.ActiveTab = "Checked";
                else
                {
                    ViewBag.VerifyMPinMessage = $"Member profile associated with Mobile:{_viewModel.MobileNumber} is already submitted successfully.Try with another number OR If you recived your profile activation message, please navigate to login page.";
                    return View("Index");
                }
                return View("MemberRegistration", commonViewModel);

            }
            else
            {
                ViewBag.VerifyMPinMessage = $"Mobile No or Mpin invalid. Try with correct mobile and Mpin ";
                return View("Index");
            }

        }
        public IActionResult InitialiseMemberRegistration(int MemberId)
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();
            commonViewModel.MemberId = MemberId;
            commonViewModel.ProfilePercentage = 0;
            Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(MemberId);
            MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
            if (member_Personal.MemberId > 0)
            {
                perosnalInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage += 20;
            }
            else
                perosnalInfoViewModel.IsNew = true;
            perosnalInfoViewModel.MemberId = MemberId;
            perosnalInfoViewModel.BirthDate = perosnalInfoViewModel.BirthDate == DateTime.MinValue ? DateTime.Now.AddYears(-72) : perosnalInfoViewModel.BirthDate;

            commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;

            Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(MemberId);
            MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
            if (member_contact.MemberId > 0)
            {
                contactInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage += 20;
            }
            else
                contactInfoViewModel.IsNew = true;
            contactInfoViewModel.MemberId = MemberId;
            commonViewModel.MemberConatctInfo = contactInfoViewModel;

            Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(MemberId);
            MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
            educationEmploymentInfoViewModel.MemberId = MemberId;
            if (member_education.MemberId > 0)
            {
                educationEmploymentInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage += 20;
            }
            else
                educationEmploymentInfoViewModel.IsNew = true;

            commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;

            List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(MemberId);
            List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
            foreach (Member_FamilyDetails family in member_Family)
            {
                memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
            }
            MemberFamilyInfoViewModel familyInfoViewModel = new MemberFamilyInfoViewModel();
            familyInfoViewModel.MemberId = MemberId;
            familyInfoViewModel.IsNew = true;
            familyInfoViewModel.DOB = familyInfoViewModel.DOB == DateTime.MinValue ? DateTime.Now.AddYears(-72) : familyInfoViewModel.DOB;
            familyInfoViewModel.MemberFamilyDetails = memberFamilies;
            commonViewModel.MemberFamilyInfo = familyInfoViewModel;
            if (memberFamilies.Count > 0)
                commonViewModel.ProfilePercentage += 20;
            //    ViewBag.MemberList = memberFamilies;

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(MemberId);
            MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
            paymentViewModel.MemberId = MemberId;
            commonViewModel.MemberPaymentInfo = paymentViewModel;

            if (member_Personal.MemberId == 0)
                perosnalInfoViewModel.ActiveTab = "Checked";
            else if (member_contact.MemberId == 0)
                contactInfoViewModel.ActiveTab = "Checked";
            else if (member_education.MemberId == 0)
                educationEmploymentInfoViewModel.ActiveTab = "Checked";
            else if (memberFamilies.Count == 0 || member_Payments.MemberId > 0|| member_Payments.MemberId == 0)
                ViewBag.FamilyTab = "Checked";
            //else if (member_Payments.MemberId == 0)
            //    paymentViewModel.ActiveTab = "Checked";

            return View("MemberRegistration", commonViewModel);
        }
        public IActionResult InitialiseMemberRegistrationForFamily(MemberFamilyInfoViewModel familyModel)
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();
            commonViewModel.MemberId = familyModel.MemberId;
            commonViewModel.ProfilePercentage = 0;
            int MemberId = familyModel.MemberId;
            Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(MemberId);
            MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
            if (member_Personal.MemberId > 0)
            {
                perosnalInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage += 20;
            }
            else
                perosnalInfoViewModel.IsNew = true;
            perosnalInfoViewModel.MemberId = MemberId;
            perosnalInfoViewModel.BirthDate = perosnalInfoViewModel.BirthDate == DateTime.MinValue ? DateTime.Now.AddYears(-72) : perosnalInfoViewModel.BirthDate;

            commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;

            Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(MemberId);
            MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
            if (member_contact.MemberId > 0)
            {
                contactInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage += 20;
            }
            else
                contactInfoViewModel.IsNew = true;
            contactInfoViewModel.MemberId = MemberId;
            commonViewModel.MemberConatctInfo = contactInfoViewModel;

            Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(MemberId);
            MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
            educationEmploymentInfoViewModel.MemberId = MemberId;
            if (member_education.MemberId > 0)
            {
                educationEmploymentInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage += 20;
            }
            else
                educationEmploymentInfoViewModel.IsNew = true;

            commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;

            List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(MemberId);
            List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
            foreach (Member_FamilyDetails family in member_Family)
            {
                memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
            }
            //MemberFamilyInfoViewModel familyInfoViewModel = new MemberFamilyInfoViewModel();
            //familyInfoViewModel.MemberId = MemberId;
            familyModel.DOB = familyModel.DOB == DateTime.MinValue ? DateTime.Now.AddYears(-72) : familyModel.DOB;
            familyModel.MemberFamilyDetails = memberFamilies;
            familyModel.IsNew = false;
            commonViewModel.MemberFamilyInfo = familyModel;
            if (memberFamilies.Count > 0)
                commonViewModel.ProfilePercentage += 20;
            //    ViewBag.MemberList = memberFamilies;

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(MemberId);
            MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
            paymentViewModel.MemberId = MemberId;
            commonViewModel.MemberPaymentInfo = paymentViewModel;

            if (member_Personal.MemberId == 0)
                perosnalInfoViewModel.ActiveTab = "Checked";
            else if (member_contact.MemberId == 0)
                contactInfoViewModel.ActiveTab = "Checked";
            else if (member_education.MemberId == 0)
                educationEmploymentInfoViewModel.ActiveTab = "Checked";
            else if (memberFamilies.Count == 0 || member_Payments.MemberId > 0 || member_Payments.MemberId == 0)
                ViewBag.FamilyTab = "Checked";
            //else if (member_Payments.MemberId == 0)
            //    paymentViewModel.ActiveTab = "Checked";

            return View("MemberRegistration", commonViewModel);
        }
        [HttpPost]
        public IActionResult MemberEduEmpInfo(MemberFormCommonViewModel model)
        {
            Member_EducationEmploymentDetails member_EducationEmployment = mapper.Map<Member_EducationEmploymentDetails>(model.MemberEducationEmploymentInfo);
            member_EducationEmployment.CreateDate = DateTime.Now;
            member_EducationEmployment.UpdateDate = DateTime.Now;

            if (model.MemberEducationEmploymentInfo.IsNew)
                educationEmploymentDataFactory.AddDetails(member_EducationEmployment);
            else
                educationEmploymentDataFactory.UpdateDetails(member_EducationEmployment);



            return RedirectToAction("InitialiseMemberRegistration", new { MemberId = model.MemberEducationEmploymentInfo.MemberId });

        }
        public IActionResult EditFamilyMember(int id)
        {
            Member_FamilyDetails member_Family = familyDetailsDataFactory.GetDetailsByMemberId(id);
            MemberFamilyInfoViewModel model = mapper.Map<MemberFamilyInfoViewModel>(member_Family);

            return RedirectToAction("InitialiseMemberRegistrationForFamily", model);
        }
        public IActionResult DeleteFamilyMember(int id, int memberId)
        {
            familyDetailsDataFactory.DeleteById(id);

            return RedirectToAction("InitialiseMemberRegistration", new { MemberId =memberId });
        }
        [HttpPost]
        public IActionResult MemberPersonalInfo(MemberFormCommonViewModel model)
        {
            Member_PersonalDetails personalDetails = mapper.Map<Member_PersonalDetails>(model.MemberPersonalInfo);
            //personalDetails.CreateDate = DateTime.Now;
            MemberDataFactory.UpdateName(personalDetails.MemberId, personalDetails.FirstName, personalDetails.MiddleName, personalDetails.LastName);
            if (model.MemberPersonalInfo.IsNew)
                personalDataFactory.AddMemberPersonalDetails(personalDetails);
            else
                personalDataFactory.UpdateMemberPersonalDetails(personalDetails);
            return RedirectToAction("InitialiseMemberRegistration", new { MemberId = model.MemberPersonalInfo.MemberId });
        }
        public IActionResult UploadProfileImage(MemberFormCommonViewModel commonViewModel)
        {
            string path = Path.Combine(this.Environment.WebRootPath, "MemberProfileImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string FilePath = $"~/MemberProfileImages/{commonViewModel.file.FileName}";
            string fileName = Path.GetFileName(commonViewModel.file.FileName);
            string fullPath = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                commonViewModel.file.CopyTo(stream);

            }
            personalDataFactory.UpdateMemberProfileImage(commonViewModel.MemberId, FilePath);

            return RedirectToAction("InitialiseMemberRegistration", new { MemberId = commonViewModel.MemberId });
        }
        [HttpPost]
        public IActionResult MemberContactInfo(MemberFormCommonViewModel model)
        {
            Member_ContactDetails member_ContactDetails = mapper.Map<Member_ContactDetails>(model.MemberConatctInfo);
            member_ContactDetails.CreateDate = DateTime.Now;
            member_ContactDetails.UpdateDate = DateTime.Now;
            if (model.MemberConatctInfo.IsNew)
                contactDetailsDataFactory.AddDetails(member_ContactDetails);
            else
                contactDetailsDataFactory.UpdateDetails(member_ContactDetails);
            return RedirectToAction("InitialiseMemberRegistration", new { MemberId = model.MemberConatctInfo.MemberId });
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
            

            return RedirectToAction("InitialiseMemberRegistration", new { MemberId = model.MemberFamilyInfo.MemberId });
        }
        [HttpPost]
        public IActionResult MemberFamilyInfo(MemberFormCommonViewModel model)
        {
            foreach (var family in model.MemberFamilyInfo.MemberFamilyDetails)
            {
                Member_FamilyDetails _FamilyDetails = new Member_FamilyDetails()
                {
                    MemberID = model.MemberId,
                    Name = family.Name,
                    Occupation = family.Occupation,
                    Education = family.Education,
                    DOB = Convert.ToDateTime(family.DOB),
                    BloodGroup = family.BloodGroup,
                    CreateDate = DateTime.Now

                };
                familyDetailsDataFactory.AddDetails(_FamilyDetails);

               

            }
            return RedirectToAction("InitialiseMemberRegistration", new { MemberId = model.MemberFamilyInfo.MemberId });
        }

        public IActionResult GoToPayment(MemberFormCommonViewModel model)
        {
            var data = (from memberbasic in dBContext.Member_PersonalDetails
                        join memberContact in dBContext.Member_ContactDetails
                         on memberbasic.MemberId equals memberContact.MemberId
                        where memberContact.MemberId == model.MemberPaymentInfo.MemberId
                        select new
                        {
                            Name = $"{memberbasic.FirstName} {memberbasic.MiddleName} {memberbasic.LastName}",
                            Conatct = memberContact.Mobile1,
                            Email = memberContact.EmailId
                        }
                           ).FirstOrDefault();
            MemberPaymentViewModel memberPayment = new MemberPaymentViewModel()
            {
                MemberId =model.MemberPaymentInfo.MemberId,
                MemberName = data.Name,
                Mobile = data.Conatct,
                Email = data.Email,
                Amount = Convert.ToInt32(configuration.GetSection("SubscriptionCharges").Value)
            };

            return RedirectToAction("AcceptMemberPayment", "Payment", memberPayment);
        }
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
