﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SBMMember.Data;
using SBMMember.Data.DataFactory;
using SBMMember.Models;
using SBMMember.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SBMMember.Web.Controllers
{
    public class MemberController : Controller
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
        public MemberController(IMemberDataFactory dataFactory,
            IMemberPersonalDataFactory memberPersonalDataFactory,
            IMemberContactDetailsDataFactory memberContactDetailsDataFactory,
            IMemberEducationEmploymentDataFactory memberEducationEmploymentDataFactory,
            IMemberFamilyDetailsDataFactory memberFamilyDetailsDataFactory,
            ILogger<MemberController> logger,
            IMapper Mapper,
            SBMMemberDBContext memberDBContext)
        {
            Logger = logger;
            mapper = Mapper;
            memberDataFactory = dataFactory;
            personalDataFactory = memberPersonalDataFactory;
            contactDetailsDataFactory = memberContactDetailsDataFactory;
            educationEmploymentDataFactory = memberEducationEmploymentDataFactory;
            familyDetailsDataFactory = memberFamilyDetailsDataFactory;
            dBContext = memberDBContext;

        }

        public IActionResult SetPasswordAndPin()
        {

            return View();
        }
        [HttpPost]
        public IActionResult SetPasswordAndPin(LoginViewModel model)
        {
            Members members = new Members()
            {
                Mpin = model.MPIN,
                Mobile = HttpContext.Session.GetString("Mobile"),
                Password = model.Password,
                Status = "Active",
                Createdate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            var member = memberDataFactory.AddMember(members);
            if (member != null && member.MemberId > 0)
            {
                MemberPerosnalInfoViewModel perosnalInfoViewModel = new MemberPerosnalInfoViewModel()
                {
                    MemberId = member.MemberId
                };
                return View("MemberPersonalInfo", perosnalInfoViewModel);
            }
            else
                return View();
        }

        public IActionResult MemberPersonalInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MemberPersonalInfo(MemberPerosnalInfoViewModel model)
        {

            Member_PersonalDetails personalDetails = mapper.Map<Member_PersonalDetails>(model);
            //personalDetails.CreateDate = DateTime.Now;
            var response = personalDataFactory.AddMemberPersonalDetails(personalDetails);
            if (response.Result == "Success")
            {
                MemberContactInfoViewModel viewModel = new MemberContactInfoViewModel()
                {
                    MemberId = model.MemberId
                };
                return View("MemberContactInfo", viewModel);
            }
            else
                return View();

        }
        public IActionResult MemberContactInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MemberContactInfo(MemberContactInfoViewModel model)
        {
            Member_ContactDetails member_ContactDetails = mapper.Map<Member_ContactDetails>(model);
            member_ContactDetails.CreateDate = DateTime.Now;
            member_ContactDetails.UpdateDate = DateTime.Now;
            var response = contactDetailsDataFactory.AddDetails(member_ContactDetails);
            if (response.Result == "Success")
            {
                MemberEducationEmploymentInfoViewModel viewModel = new MemberEducationEmploymentInfoViewModel()
                {
                    MemberId = model.MemberId
                };
                return View("MemberEduEmpInfo", viewModel);
            }
            else
                return View();
        }
        public IActionResult MemberEduEmpInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MemberEduEmpInfo(MemberEducationEmploymentInfoViewModel model)
        {
            Member_EducationEmploymentDetails member_EducationEmployment = mapper.Map<Member_EducationEmploymentDetails>(model);
            member_EducationEmployment.CreateDate = DateTime.Now;
            member_EducationEmployment.UpdateDate = DateTime.Now;

            var response = educationEmploymentDataFactory.AddDetails(member_EducationEmployment);


            if (response.Result == "Success")
            {
                MemberFamilyInfoViewModel viewModel = new MemberFamilyInfoViewModel()
                {
                    MemberId = model.MemberId
                };
                return View("MemberFamilyInfo", viewModel);
            }
            else
                return View();
        }
        public IActionResult MemberFamilyInfo()
        {
            MemberFamilyInfoViewModel model = new MemberFamilyInfoViewModel();

            return View(model);
        }
        [HttpPost]
        public IActionResult AddToList(MemberFamilyInfoViewModel model)
        {
            if (Request.Method == HttpMethods.Post)
            {
                memberFamilies.Add(model);
                ViewBag.MemberList = memberFamilies;
            }
            ModelState.Clear();
            MemberFamilyInfoViewModel Newmodel = new MemberFamilyInfoViewModel()
            {
                MemberId = model.MemberId
            };

            return View("MemberFamilyInfo", Newmodel);
        }
        [HttpPost]
        public IActionResult MemberFamilyInfo(MemberFamilyInfoViewModel model)
        {
            foreach (var family in memberFamilies)
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
            var data = (from memberbasic in dBContext.Member_PersonalDetails
                        join memberContact in dBContext.Member_ContactDetails
                         on memberbasic.MemberId equals memberContact.MemberId
                        where memberContact.MemberId == model.MemberId
                        select new
                        {
                            Name = $"{memberbasic.FirstName} {memberbasic.MiddleName} {memberbasic.LastName}",
                            Conatct = memberContact.Mobile1,
                            Email = memberContact.EmailId
                        }
                           ).FirstOrDefault();
            MemberPaymentViewModel memberPayment = new MemberPaymentViewModel()
            {
                MemberName = data.Name,
                Mobile = data.Conatct,
                Email = data.Email,
                Amount = 1000
            };

            return RedirectToAction("AcceptMemberPayment","Payment",memberPayment);
        }

        public IActionResult MemberPaymentAndRecieptInfo()
        {
            return View();
        }
    }
}
