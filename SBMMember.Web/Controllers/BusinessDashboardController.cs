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

namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class BusinessDashboardController : Controller
    {
        private readonly IMemberBusinessDataFactory businessDataFactory;
        private readonly IMapper mapper;
        public BusinessDashboardController(IMemberBusinessDataFactory dataFactory,IMapper _mapper)
        {
            businessDataFactory = dataFactory;
            mapper = _mapper;
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
            businessDataFactory.AddDetails(member_Business);
                
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
            MemberBusinessViewModel businessViewModel = new MemberBusinessViewModel() {
                MemberBusinesses = businesses
            };
            return View(businessViewModel);
        }
    }
}
