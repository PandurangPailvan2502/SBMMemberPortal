using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Models;
using SBMMember.Data.DataFactory;
using AutoMapper;

namespace SBMMember.Web.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IMemberBusinessDataFactory businessDataFactory;
        private readonly IJobPostingDataFactory jobPostingDataFactory;
        private readonly IMapper mapper;
        public AdminDashboardController(IMemberBusinessDataFactory dataFactory, IMapper _mapper,IJobPostingDataFactory _jobPostingDataFactory)
        {
            businessDataFactory = dataFactory;
            mapper = _mapper;
            jobPostingDataFactory = _jobPostingDataFactory;
        }
        public IActionResult AdminHome()
        {
            return View();
        }

        public IActionResult JobPost()
        {
            return View();
        }
        [HttpPost]
        public IActionResult JobPost(JobPostingViewModel model)
        {
            JobPostings job = mapper.Map<JobPostings>(model);
            jobPostingDataFactory.AddJobDetails(job);

            ModelState.Clear();
            return View();
        }

        public IActionResult AddBusiness()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBusiness(MemberBusinessViewModel model)
        {
            Member_BusinessDetails member_Business = mapper.Map<Member_BusinessDetails>(model);
            var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            int MemberId = Convert.ToInt32(memberId);
            member_Business.MemberId = MemberId;
            businessDataFactory.AddDetails(member_Business);

            return View();
        }
    }
}
