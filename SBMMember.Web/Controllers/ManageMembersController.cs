using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SBMMember.Data.DataFactory;
using SBMMember.Web.Models;
using SBMMember.Models;
using AutoMapper;

namespace SBMMember.Web.Controllers
{
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
        public ManageMembersController(IMemberDataFactory dataFactory,
            IMemberSearchDataFactory searchDataFactory,
            IMemberPersonalDataFactory _personalDataFactory,
            IMemberContactDetailsDataFactory _contactDetailsDataFactory,
            IMemberEducationEmploymentDataFactory _educationEmploymentDataFactory,
            IMemberFamilyDetailsDataFactory _familyDetailsDataFactory,
            IMemberPaymentsDataFactory _paymentsDataFactory,
            IMapper _mapper
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
            ViewBag.MemberList = memberFamilies;

            Member_PaymentsAndReciepts member_Payments = paymentsDataFactory.GetDetailsByMemberId(_MemberId);
            MemberPaymentRecieptsViewModel paymentViewModel = mapper.Map<MemberPaymentRecieptsViewModel>(member_Payments);
            paymentViewModel.LastMemberShipId = paymentsDataFactory.LastMembershipNo();
            commonViewModel.MemberPaymentInfo = paymentViewModel;

            return View(commonViewModel);
            //return  RedirectToAction("NewMemberList");
        }
        public IActionResult ApproveMemberProfile(int memberId)
        {
            return RedirectToAction("NewMemberList");
        }

        [HttpPost]
        public IActionResult MemberPersonalInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_PersonalDetails member_Personal = mapper.Map<Member_PersonalDetails>(formCommonViewModel.MemberPersonalInfo);
            personalDataFactory.UpdateMemberPersonalDetails(member_Personal);
            return RedirectToAction("VerifyMemberProfile", new { MemberId = formCommonViewModel.MemberPersonalInfo.MemberId });
        }
        [HttpPost]
        public IActionResult MemberContactInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_ContactDetails member_Contact = mapper.Map<Member_ContactDetails>(formCommonViewModel.MemberConatctInfo);
            contactDetailsDataFactory.UpdateDetails(member_Contact);
            return RedirectToAction("VerifyMemberProfile",new { MemberId = formCommonViewModel.MemberConatctInfo.MemberId });
        }

        [HttpPost]
        public IActionResult MemberEduEmpInfo(MemberFormCommonViewModel formCommonViewModel)
        {
            Member_EducationEmploymentDetails member_Education = mapper.Map<Member_EducationEmploymentDetails>(formCommonViewModel.MemberEducationEmploymentInfo);
            educationEmploymentDataFactory.UpdateDetails(member_Education);

            return RedirectToAction("VerifyMemberProfile", formCommonViewModel.MemberEducationEmploymentInfo.MemberId);
        }
    }
}
