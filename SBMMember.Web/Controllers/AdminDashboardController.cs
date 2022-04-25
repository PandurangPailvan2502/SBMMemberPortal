﻿using Microsoft.AspNetCore.Mvc;
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
using SBMMember.Web.Helper;
using Microsoft.AspNetCore.Authorization;

namespace SBMMember.Web.Controllers
{
    //[Authorize]
    public class AdminDashboardController : Controller
    {
        private readonly IMemberBusinessDataFactory businessDataFactory;
        private readonly IJobPostingDataFactory jobPostingDataFactory;
        private readonly IEventDataFactory eventDataFactory;
        private readonly IEventAdsDataFactory eventAdsDataFactory;
        private readonly IMapper mapper;
        private IWebHostEnvironment Environment;
        private readonly IUpcomingEventsDataFactory upcomingEventsDataFactory;
        public AdminDashboardController(IMemberBusinessDataFactory dataFactory, IMapper _mapper, IJobPostingDataFactory _jobPostingDataFactory, IEventDataFactory _eventDataFactory, IWebHostEnvironment hostEnvironment,
            IEventAdsDataFactory adsDataFactory,IUpcomingEventsDataFactory _upcomingEventsDataFactory)
        {
            businessDataFactory = dataFactory;
            mapper = _mapper;
            jobPostingDataFactory = _jobPostingDataFactory;
            eventDataFactory = _eventDataFactory;
            Environment = hostEnvironment;
            eventAdsDataFactory = adsDataFactory;
            upcomingEventsDataFactory = _upcomingEventsDataFactory;
        }
        public IActionResult AdminHome()
        {
            return View();
        }

        public IActionResult JobPost()
        {
            List<JobPostings> jobPostings = jobPostingDataFactory.GetJobPostings();
            List<JobPostingViewModel> jobs = new List<JobPostingViewModel>();
            foreach (JobPostings job in jobPostings)
            {
                JobPostingViewModel model = mapper.Map<JobPostingViewModel>(job);
                jobs.Add(model);
            }
            JobPostingViewModel postingViewModel = new JobPostingViewModel()
            {
                JobPostings = jobs
            };
            return View(postingViewModel);

        }
        public IActionResult DeleteJobPosting(int Id)
        {
            jobPostingDataFactory.DeleteJobDetails(Id);
            return RedirectToAction("JobPost");
        }
        public IActionResult JobEdit(int Id)
        {
            JobPostings job = jobPostingDataFactory.GetJobDetails(Id);
            JobPostingViewModel model = mapper.Map<JobPostingViewModel>(job);
            ViewBag.EditMode = "Yes";
            return View("JobEdit", model);
        }
        [HttpPost]
        public IActionResult JobEdit(JobPostingViewModel model)
        {
            JobPostings job = mapper.Map<JobPostings>(model);
            jobPostingDataFactory.UpdateJobDetails(job);

            ModelState.Clear();
            return RedirectToAction("JobPost");
        }
        [HttpPost]
        public IActionResult JobPost(JobPostingViewModel model)
        {
            JobPostings job = mapper.Map<JobPostings>(model);
            jobPostingDataFactory.AddJobDetails(job);

            ModelState.Clear();
            List<JobPostings> jobPostings = jobPostingDataFactory.GetJobPostings();
            List<JobPostingViewModel> jobs = new List<JobPostingViewModel>();
            foreach (JobPostings _job in jobPostings)
            {
                JobPostingViewModel _model = mapper.Map<JobPostingViewModel>(_job);
                jobs.Add(_model);
            }
            JobPostingViewModel postingViewModel = new JobPostingViewModel()
            {
                JobPostings = jobs
            };
            return View(postingViewModel);
        }

        public IActionResult AddBusiness()
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
        [HttpPost]
        public IActionResult AddBusiness(MemberBusinessViewModel model)
        {
            Member_BusinessDetails member_Business = mapper.Map<Member_BusinessDetails>(model);
            var memberId = User.Claims?.FirstOrDefault(x => x.Type.Equals("MemberId", StringComparison.OrdinalIgnoreCase))?.Value;
            int MemberId = Convert.ToInt32(memberId);
            member_Business.MemberId = MemberId;
            businessDataFactory.AddDetails(member_Business);

            //return View();
            List<Member_BusinessDetails> buisnessList = businessDataFactory.GetAllBusinessDetails();
            List<MemberBusinessViewModel> businesses = new List<MemberBusinessViewModel>();
            foreach (Member_BusinessDetails item in buisnessList)
            {
                MemberBusinessViewModel models = mapper.Map<MemberBusinessViewModel>(item);
                businesses.Add(models);
            }
            MemberBusinessViewModel businessViewModel = new MemberBusinessViewModel()
            {
                MemberBusinesses = businesses
            };
            return View(businessViewModel);
        }
        public IActionResult DeleteBusiness(int Id)
        {
            businessDataFactory.DeleteDetailsById(Id);
            return RedirectToAction("AddBusiness");
        }
        public IActionResult EditBusiness(int Id)
        {
            Member_BusinessDetails businessDetails = businessDataFactory.GetDetailsById(Id);

            MemberBusinessViewModel models = mapper.Map<MemberBusinessViewModel>(businessDetails);
            return View(models);
        }
        [HttpPost]
        public IActionResult EditBusiness(MemberBusinessViewModel model)
        {
            Member_BusinessDetails member_Business = mapper.Map<Member_BusinessDetails>(model);
            businessDataFactory.UpdateDetails(member_Business);
            return RedirectToAction("AddBusiness");
        }
        public IActionResult AddEvent()
        {
            EventViewModel viewModel = new EventViewModel()
            {
                EventInfos = eventDataFactory.GetAllEventList()
            };
            //viewModel.EventNameM = TranslationHelper.Translate("Samata Bhratru");
            return View(viewModel);
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
                    Status = "Active",
                    CreateDate = DateTime.Now,
                    DocType = "jpg"
                };
                eventDataFactory.AddEventGallery(gallery);
            }
            ModelState.Clear();
            return View();
        }
        public IActionResult DeleteEvent(int Id)
        {
            eventDataFactory.DeleteEventInfo(Id);
            return RedirectToAction("AddEvent");
        }
        public IActionResult EditEvent(int Id)
        {
            var eventInfo = eventDataFactory.GetEventInfoByeventId(Id);
            EventViewModel detailsViewModel = new EventViewModel()
            {
                EventId = eventInfo.EventId,
                EventName = eventInfo.EventName,
                EventDescription = eventInfo.EventDescription,
                EventYear = eventInfo.EventYear,
                eventGalleries = eventDataFactory.GetGalleryphotosByeventId(Id)
            };
            return View(detailsViewModel);

        }
        [HttpPost]
        public IActionResult EditEvent(EventViewModel model)
        {
            EventInfo eventInfo = new EventInfo()
            {
                EventName = model.EventName,
                EventDescription = model.EventDescription,
                EventYear = model.EventYear,
                EventId=model.EventId
              
            };
            eventDataFactory.UpdateEventInfo(eventInfo);
            return RedirectToAction("AddEvent");
        }
        public IActionResult ManageMembers()
        {
            return View();
        }
        public IActionResult AddEventAds()
        {
            EventAdsViewModel adsViewModel = new EventAdsViewModel()
            {
                EventAds = eventAdsDataFactory.GetAllEventAds()
            };
            return View(adsViewModel);
        }
        [HttpPost]
        public IActionResult AddEventAds(EventAdsViewModel model)
        {
            string path = Path.Combine(this.Environment.WebRootPath, "EventAds");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            EventAds eventAds = new EventAds()
            {
                EventTitle = model.EventTitle,
                EventYear = model.EventYear,
                FilePath = $"~/EventAds/{model.file.FileName}",
                Status = "Active",
                CreateDate = DateTime.Now
            };

            string fileName = Path.GetFileName(model.file.FileName);
            string fullPath = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                model.file.CopyTo(stream);

            }
            eventAdsDataFactory.AddEventAds(eventAds);
            ModelState.Clear();
            EventAdsViewModel adsViewModel = new EventAdsViewModel()
            {
                EventAds = eventAdsDataFactory.GetAllEventAds()
            };
            return View(adsViewModel);
        }
        public IActionResult DeleteEventAd(int Id)
        {
            eventAdsDataFactory.DeleteEventAds(Id);
            return RedirectToAction("AddEventAds");
        }
        public IActionResult EditEventAd(int Id)
        {
            EventAds eventAds = eventAdsDataFactory.GetEventAdById(Id);
            EventAdsViewModel model = mapper.Map<EventAdsViewModel>(eventAds);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditEventAds(EventAdsViewModel model)
        {
            EventAds eventAds = new EventAds()
            {
                Id=model.Id,
                EventTitle = model.EventTitle,
                EventYear = model.EventYear,
                FilePath = model.file != null ? $"~/EventAds/{model.file.FileName}" : model.FilePath
            };
            eventAdsDataFactory.UpdateEventAds(eventAds);
            if(model.file!=null)
            {
                string path = Path.Combine(this.Environment.WebRootPath, "EventAds");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(model.file.FileName);
                string fullPath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    model.file.CopyTo(stream);

                }
            }
            return RedirectToAction("AddEventAds");
        }
        [HttpGet]
        public ActionResult GetPdf(string filepath)
        {
            //string filePath = "~/file/test.pdf";
            Response.Headers.Add("Content-Disposition", $"inline; filename={filepath.Split('/')[2]}");
            return File(filepath, "application/pdf");
        }

        public IActionResult ManageEvents()
        {
            return View();
        }

      public IActionResult UpcomingEvents()
        {
            UpcomingEventsViewModel model = new UpcomingEventsViewModel();
            model.EventDate =DateTime.Today;
            model.EventInfos = upcomingEventsDataFactory.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult UpcomingEvents(UpcomingEventsViewModel model)
        {
            UpcomingEvent upcomingEvents = mapper.Map<UpcomingEvent>(model);
            upcomingEventsDataFactory.AddDetails(upcomingEvents);
            return RedirectToAction("UpcomingEvents");
        }
        public IActionResult EditUpcomingEvent(int id)
        {
            UpcomingEvent evnt = upcomingEventsDataFactory.GetDetailsById(id);
            UpcomingEventsViewModel viewModel = mapper.Map<UpcomingEventsViewModel>(evnt);
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult EditUpcomingEvent(UpcomingEventsViewModel viewModel)
        {

            UpcomingEvent upcoming = mapper.Map<UpcomingEvent>(viewModel);
            upcomingEventsDataFactory.UpdateDetails(upcoming);
            return RedirectToAction("UpcomingEvents");
        }

        public IActionResult DeleteUpcomingEvent(int id)
        {
            upcomingEventsDataFactory.DeleteById(id);
            return RedirectToAction("UpcomingEvents");
        }
    }
}
