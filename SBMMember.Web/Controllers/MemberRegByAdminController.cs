using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SBMMember.Data.DataFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Models;
using Microsoft.AspNetCore.Http;
using SBMMember.Web.Helper;

namespace SBMMember.Web.Controllers
{
    public class MemberRegByAdminController : Controller
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


        public MemberRegByAdminController(IMemberDataFactory dataFactory,
           IMemberSearchDataFactory searchDataFactory,
           IMemberPersonalDataFactory _personalDataFactory,
           IMemberContactDetailsDataFactory _contactDetailsDataFactory,
           IMemberEducationEmploymentDataFactory _educationEmploymentDataFactory,
           IMemberFamilyDetailsDataFactory _familyDetailsDataFactory,
           IMemberPaymentsDataFactory _paymentsDataFactory,
           IMapper _mapper,
           IMemberFormStatusDataFactory _formStatusDataFactory
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
        }
        public IActionResult MemberRegistration()
        {
            MemberFormCommonViewModel viewModel = new MemberFormCommonViewModel();
            MemberPerosnalInfoViewModel _MemberPersonalInfo = new MemberPerosnalInfoViewModel();
            _MemberPersonalInfo.ActiveTab = "Checked";
            _MemberPersonalInfo.BirthDate = DateTime.Now.AddYears(-72);
            _MemberPersonalInfo.TabValue = "Tab1";
            _MemberPersonalInfo.IsNew = true;
            viewModel.ProfilePercentage = "0";
            viewModel.MemberPersonalInfo = _MemberPersonalInfo;
            viewModel.MemberConatctInfo = new MemberContactInfoViewModel();
            viewModel.MemberEducationEmploymentInfo = new MemberEducationEmploymentInfoViewModel();
            viewModel.MemberPaymentInfo = new MemberPaymentRecieptsViewModel();
            viewModel.MemberFamilyInfo = new MemberFamilyInfoViewModel();

            return View(viewModel);
        }
        public IActionResult ReInitialiseMemberForm(int MemberId)
        {
            MemberFormCommonViewModel commonViewModel = new MemberFormCommonViewModel();
            commonViewModel.MemberId = MemberId;
            Member_PersonalDetails member_Personal = personalDataFactory.GetMemberPersonalDetailsByMemberId(MemberId);
            MemberPerosnalInfoViewModel perosnalInfoViewModel = mapper.Map<MemberPerosnalInfoViewModel>(member_Personal);
            if (member_Personal.MemberId > 0)
            {
                perosnalInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage = "20"; 
            }
            else
                perosnalInfoViewModel.IsNew = true;
            perosnalInfoViewModel.MemberId = MemberId;
            perosnalInfoViewModel.TabValue = "tab1";
            perosnalInfoViewModel.BirthDate = perosnalInfoViewModel.BirthDate == DateTime.MinValue ? DateTime.Now.AddYears(-72) : perosnalInfoViewModel.BirthDate;
           

            commonViewModel.MemberPersonalInfo = perosnalInfoViewModel;

            Member_ContactDetails member_contact = contactDetailsDataFactory.GetDetailsByMemberId(MemberId);
            MemberContactInfoViewModel contactInfoViewModel = mapper.Map<MemberContactInfoViewModel>(member_contact);
            if (member_contact.MemberId > 0)
            {
                contactInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage = "40";
            }
            else
                contactInfoViewModel.IsNew = true;
            contactInfoViewModel.MemberId = MemberId;
            contactInfoViewModel.TabValue = "tab2";

            commonViewModel.MemberConatctInfo = contactInfoViewModel;

            Member_EducationEmploymentDetails member_education = educationEmploymentDataFactory.GetDetailsByMemberId(MemberId);
            MemberEducationEmploymentInfoViewModel educationEmploymentInfoViewModel = mapper.Map<MemberEducationEmploymentInfoViewModel>(member_education);
            educationEmploymentInfoViewModel.MemberId = MemberId;
            if (member_education.MemberId > 0)
            {
                educationEmploymentInfoViewModel.IsNew = false;
                commonViewModel.ProfilePercentage = "60";
            }
            else
                educationEmploymentInfoViewModel.IsNew = true;
            educationEmploymentInfoViewModel.TabValue = "tab3";
            commonViewModel.MemberEducationEmploymentInfo = educationEmploymentInfoViewModel;

            List<Member_FamilyDetails> member_Family = familyDetailsDataFactory.GetFamilyDetailsByMemberId(MemberId);
            List<MemberFamilyInfoViewModel> memberFamilies = new List<MemberFamilyInfoViewModel>();
            foreach (Member_FamilyDetails family in member_Family)
            {
                memberFamilies.Add(mapper.Map<MemberFamilyInfoViewModel>(family));
            }
            MemberFamilyInfoViewModel familyInfoViewModel = new MemberFamilyInfoViewModel();
            familyInfoViewModel.MemberId = MemberId;
            familyInfoViewModel.DOB = familyInfoViewModel.DOB == DateTime.MinValue ? DateTime.Now.AddYears(-72) : familyInfoViewModel.DOB;
            familyInfoViewModel.MemberFamilyDetails = memberFamilies;
            familyInfoViewModel.TabValue = "tab4";
            commonViewModel.MemberFamilyInfo = familyInfoViewModel;
            if (memberFamilies.Count > 0)
                commonViewModel.ProfilePercentage = "80";
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

            return View("MemberRegistration", commonViewModel);
        }
        [HttpPost]
        public IActionResult MemberPersonalInfo(MemberFormCommonViewModel model)
        {

            Member_PersonalDetails personalDetails = mapper.Map<Member_PersonalDetails>(model.MemberPersonalInfo);
            //personalDetails.CreateDate = DateTime.Now;
            //MemberDataFactory.UpdateName(personalDetails.MemberId, personalDetails.FirstName, personalDetails.MiddleName, personalDetails.LastName);

            if (model.MemberPersonalInfo.IsNew)
            {
                Members members = new Members()
                {
                  FirstName=model.MemberPersonalInfo.FirstName,
                  LastName=model.MemberPersonalInfo.LastName,
                  MiddleName=model.MemberPersonalInfo.MiddleName,
                 Status="Active",
                 Createdate=DateTime.Now,
                 UpdateDate=DateTime.Now
                 
                };
                var member = memberDataFactory.AddMember(members);
                personalDetails.MemberId = member.MemberId;
                personalDataFactory.AddMemberPersonalDetails(personalDetails);
            }
            else
                personalDataFactory.UpdateMemberPersonalDetails(personalDetails);
            //return RedirectToAction("InitialiseMemberRegistration", new { MemberId = model.MemberPersonalInfo.MemberId });
            return RedirectToAction("ReInitialiseMemberForm", new { MemberId = personalDetails.MemberId });
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
            Members members = new Members() { Mobile = member_ContactDetails.Mobile1 ,MemberId=model.MemberConatctInfo.MemberId};
            memberDataFactory.UpdateMobileNo(members);

            return RedirectToAction("ReInitialiseMemberForm", new { MemberId = model.MemberConatctInfo.MemberId });
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



            return RedirectToAction("ReInitialiseMemberForm", new { MemberId = model.MemberEducationEmploymentInfo.MemberId });

        }
        [HttpPost]
        public IActionResult AddToList(MemberFormCommonViewModel model)
        {
            if (Request.Method == HttpMethods.Post)
            {
                //memberFamilies.Add(model.MemberFamilyInfo);
                //ViewBag.MemberList = memberFamilies;

                Member_FamilyDetails member = new Member_FamilyDetails()
                {
                    MemberID = model.MemberFamilyInfo.MemberId,
                    Name = model.MemberFamilyInfo.Name,
                    Relation = model.MemberFamilyInfo.Relation,
                    BloodGroup = model.MemberFamilyInfo.BloodGroup,
                    Education = model.MemberFamilyInfo.Education,
                    DOB = model.MemberFamilyInfo.DOB,
                    Occupation = model.MemberFamilyInfo.Occupation
                };
                familyDetailsDataFactory.AddDetails(member);
            }
            ModelState.Clear();


            return RedirectToAction("ReInitialiseMemberForm", new { MemberId = model.MemberFamilyInfo.MemberId });
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
            return RedirectToAction("ReInitialiseMemberForm", new { MemberId = model.MemberFamilyInfo.MemberId });
        }
        [HttpPost]
        public IActionResult MemberPaymentAndReciptsInfo(MemberFormCommonViewModel model)
        {
            Member_PaymentsAndReciepts member_Payments = mapper.Map<Member_PaymentsAndReciepts>(model.MemberPaymentInfo);          

           
                paymentsDataFactory.AddDetails(member_Payments);
           

            Member_FormStatus member_Form = new Member_FormStatus()
            {
                MemberId = model.MemberPaymentInfo.MemberId,
                FormStatus = "Submitted",
                FormSubmitDate = DateTime.Now
            };
           
            formStatusDataFactory.AddDetails(member_Form);
            sendRegistrationSMS(model.MemberPaymentInfo.MemberId);
            return RedirectToActionPermanent("NewMemberList","ManageMembers");
        }
        private void sendRegistrationSMS(int memberId)
        {
            Members member = memberDataFactory.GetDetailsByMemberId(memberId);
            if (member != null && member.MemberId > 0)
            {
                string memberName = $"{member.FirstName} {member.LastName}";
                string smsTemplate = $"Dear {memberName}, Thank you for your membership registration at Samata Bhratru Mandal (PCMC Pune). Your profile is under verification & you will be notified once it is approved.";
                SMSHelper.SendSMS(member.Mobile, smsTemplate.Trim());
            }
        }
    }
}
