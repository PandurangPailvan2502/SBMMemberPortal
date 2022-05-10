using Microsoft.AspNetCore.Mvc;
using SBMMember.Web.Helper;
using SBMMember.Web.Models;
using SBMMember.Data.DataFactory;
using SBMMember.Models;
using AutoMapper;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;
namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class BusinessDashboardController : Controller
    {
        private readonly IMemberBusinessDataFactory businessDataFactory;
        private readonly IMapper mapper;
        private readonly IToastNotification _toastNotification;
        public BusinessDashboardController(IMemberBusinessDataFactory dataFactory, IMapper _mapper, IToastNotification toast)
        {
            businessDataFactory = dataFactory;
            mapper = _mapper;
            _toastNotification = toast;
        }

        public IActionResult BusinessDashboard()
        {
            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                Banners = BannerHelper.GetBanners()
            };
            return View(viewModel);
        }

        public IActionResult RegisterBusiness()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterBusiness(MemberBusinessViewModel model)
        {
            Member_BusinessDetails member_Business = mapper.Map<Member_BusinessDetails>(model);
            var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            int MemberId = Convert.ToInt32(memberId);
            member_Business.MemberId = MemberId;
          ResponseDTO response=  businessDataFactory.AddDetails(member_Business);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return View();
        }

        public IActionResult BusinessDirectory()
        {
            List<Member_BusinessDetails> buisnessList = businessDataFactory.GetAllBusinessDetails();
            List<MemberBusinessViewModel> businesses = new List<MemberBusinessViewModel>();
            foreach (Member_BusinessDetails item in buisnessList)
            {
                MemberBusinessViewModel model = mapper.Map<MemberBusinessViewModel>(item);
                businesses.Add(model);
            }
            MemberBusinessViewModel businessViewModel = new MemberBusinessViewModel()
            {
                MemberBusinesses = businesses
            };
            return View(businessViewModel);
        }
    }
}
