using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Models;
using SBMMember.Data.DataFactory;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SBMMember.Web.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IMemberBusinessDataFactory businessDataFactory;
        private readonly IJobPostingDataFactory jobPostingDataFactory;
        private readonly IEventDataFactory eventDataFactory;
        private readonly IMapper mapper;
        private IWebHostEnvironment Environment;
        public AdminDashboardController(IMemberBusinessDataFactory dataFactory, IMapper _mapper, IJobPostingDataFactory _jobPostingDataFactory, IEventDataFactory _eventDataFactory, IWebHostEnvironment hostEnvironment)
        {
            businessDataFactory = dataFactory;
            mapper = _mapper;
            jobPostingDataFactory = _jobPostingDataFactory;
            eventDataFactory = _eventDataFactory;
            Environment = hostEnvironment;
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
        public IActionResult AddEvent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddEvent(EventViewModel model)
        {
            EventInfo eventInfo = new EventInfo()
            {
                EventName = model.EventName,
                EventDescription = model.EventDescription,
                EventYear = model.EventYear,
                Status = "Active",
                CreateDate = DateTime.Now

            };
            EventInfo info = eventDataFactory.AddEventInfo(eventInfo);
            string path = Path.Combine(this.Environment.WebRootPath, "EventGallery");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (IFormFile postedFile in model.FormFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                string fullPath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);

                }
                EventGallery gallery = new EventGallery()
                {
                    FilePath = $"~/EventGallery/{fileName}",
                    EventId = info.EventId,
                    Status = "Ative",
                    CreateDate = DateTime.Now,
                    DocType = "jpg"
                };
                eventDataFactory.AddEventGallery(gallery);
            }
            ModelState.Clear();
            return View();
        }

        public IActionResult ManageMembers()
        {
            return View();
        }
        public IActionResult AddEventAds()
        {
            return View();
        }
    }
}
