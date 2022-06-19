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
using SBMMember.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
//using AspNetCoreHero.ToastNotification.Abstractions;

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
        private readonly IEventTitlesDataFactory eventTitlesDataFactory;
        // private readonly INotyfService _notyf;
        private readonly IToastNotification _toastNotification;
        private readonly IMemberMeetingsDataFactory meetingsDataFactory;
        public AdminDashboardController(IMemberBusinessDataFactory dataFactory, IMapper _mapper, IJobPostingDataFactory _jobPostingDataFactory, IEventDataFactory _eventDataFactory, IWebHostEnvironment hostEnvironment,
            IEventAdsDataFactory adsDataFactory, IUpcomingEventsDataFactory _upcomingEventsDataFactory, IEventTitlesDataFactory _eventTitlesDataFactory,
            IToastNotification toast,IMemberMeetingsDataFactory memberMeetings)
        {
            businessDataFactory = dataFactory;
            mapper = _mapper;
            jobPostingDataFactory = _jobPostingDataFactory;
            eventDataFactory = _eventDataFactory;
            Environment = hostEnvironment;
            eventAdsDataFactory = adsDataFactory;
            upcomingEventsDataFactory = _upcomingEventsDataFactory;
            eventTitlesDataFactory = _eventTitlesDataFactory;
            _toastNotification = toast;
            meetingsDataFactory = memberMeetings;
            //_notyf = notyf;
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

            ResponseDTO response = jobPostingDataFactory.DeleteJobDetails(Id);
            if (response.Result == "Success")
            {
                _toastNotification.AddSuccessToastMessage(response.Message);
            }
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("JobPost");
        }
        public IActionResult JobEdit(int Id)
        {
            JobPostings job = jobPostingDataFactory.GetJobDetails(Id);
            JobPostingViewModel model = mapper.Map<JobPostingViewModel>(job);
            ViewBag.EditMode = "Yes";
            // _notyf.Success($"{job.JobTitle} updated successfully.");

            return View("JobEdit", model);
        }
        [HttpPost]
        public IActionResult JobEdit(JobPostingViewModel model)
        {
            JobPostings job = mapper.Map<JobPostings>(model);
            jobPostingDataFactory.UpdateJobDetails(job);

            ModelState.Clear();
            // Alert($"{job.JobTitle} updated successfully.", Enum.NotificationType.success);
            _toastNotification.AddSuccessToastMessage($"{job.JobTitle} updated successfully.");
            return RedirectToAction("JobPost");
        }
        [HttpPost]
        public IActionResult JobPost(JobPostingViewModel model)
        {
            JobPostings job = mapper.Map<JobPostings>(model);
            ResponseDTO response = jobPostingDataFactory.AddJobDetails(job);
            if (response.Result == "Success")
            {
                _toastNotification.AddSuccessToastMessage(response.Message);
            }
            else
                _toastNotification.AddErrorToastMessage(response.Message);

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
            ResponseDTO response = businessDataFactory.AddDetails(member_Business);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);

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
            ResponseDTO response = businessDataFactory.DeleteDetailsById(Id);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
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
            ResponseDTO response = businessDataFactory.UpdateDetails(member_Business);

            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("AddBusiness");
        }
        public IActionResult AddEvent()
        {
            EventViewModel viewModel = new EventViewModel()
            {
                EventInfos = eventDataFactory.GetAllEventList(),
                EventTitles = eventTitlesDataFactory.GetEventTitles().Select(x => new SelectListItem() { Value = x.EventTitle, Text = x.EventTitle }).ToList(),
                EventYears = YearHelper.GetYears()
            };
            //viewModel.EventNameM = TranslationHelper.Translate("Samata Bhratru");
            viewModel.EventTitles.Insert(0, new SelectListItem() { Text = " Select Event Title ", Value = "0" });
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
            if (model.FormFiles != null)
            {
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
            }
            ModelState.Clear();
            _toastNotification.AddSuccessToastMessage("Event details added Successfully.");
            return RedirectToAction("AddEvent");
        }
        public IActionResult DeleteEvent(int Id)
        {
            eventDataFactory.DeleteEventInfo(Id);
            _toastNotification.AddSuccessToastMessage("Event Ino deleted successfully.");
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
                eventGalleries = eventDataFactory.GetGalleryphotosByeventId(Id),
                EventTitles = eventTitlesDataFactory.GetEventTitles().Select(x => new SelectListItem() { Value = x.EventTitle, Text = x.EventTitle }).ToList(),
                EventYears = YearHelper.GetYears()
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
                EventId = model.EventId

            };
            eventDataFactory.UpdateEventInfo(eventInfo);
            string path = Path.Combine(this.Environment.WebRootPath, "EventGallery");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (model.FormFiles != null)
            {
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
                        EventId = model.EventId,
                        Status = "Active",
                        CreateDate = DateTime.Now,
                        DocType = "jpg"
                    };
                    eventDataFactory.AddEventGallery(gallery);
                }
            }
            _toastNotification.AddSuccessToastMessage("EventInfo Added successfully");
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
                EventAds = eventAdsDataFactory.GetAllEventAds(),
                EventTitles = eventTitlesDataFactory.GetEventTitles().Select(x => new SelectListItem() { Value = x.EventTitle, Text = x.EventTitle }).ToList(),
                EventYears = YearHelper.GetYears()
            };
            adsViewModel.EventTitles.Insert(0, new SelectListItem() { Text = " Select Event Title ", Value = "0" });
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
            ResponseDTO response = eventAdsDataFactory.AddEventAds(eventAds);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            ModelState.Clear();
            EventAdsViewModel adsViewModel = new EventAdsViewModel()
            {
                EventAds = eventAdsDataFactory.GetAllEventAds(),
                EventTitles = eventTitlesDataFactory.GetEventTitles().Select(x => new SelectListItem() { Value = x.EventTitle, Text = x.EventTitle }).ToList(),
                EventYears = YearHelper.GetYears()
            };
            adsViewModel.EventTitles.Insert(0, new SelectListItem() { Text = " Select Event Title ", Value = "0" });
            adsViewModel.EventYears.Insert(0, new SelectListItem() { Text = " Select Event Year ", Value = "0" });
            return View(adsViewModel);
        }
        public IActionResult DeleteEventAd(int Id)
        {
            ResponseDTO response = eventAdsDataFactory.DeleteEventAds(Id);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
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
                Id = model.Id,
                EventTitle = model.EventTitle,
                EventYear = model.EventYear,
                FilePath = model.file != null ? $"~/EventAds/{model.file.FileName}" : model.FilePath
            };
            ResponseDTO response = eventAdsDataFactory.UpdateEventAds(eventAds);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            if (model.file != null)
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
            model.EventDate = DateTime.Today;
            model.EventInfos = upcomingEventsDataFactory.GetAll();
            model.EventTitles = eventTitlesDataFactory.GetEventTitles().Select(x => new SelectListItem() { Value = x.EventTitle, Text = x.EventTitle }).ToList();
            model.EventTitles.Insert(0, new SelectListItem() { Text = " Select Event Title ", Value = "0" });

            return View(model);
        }
        [HttpPost]
        public IActionResult UpcomingEvents(UpcomingEventsViewModel model)
        {
            UpcomingEvent upcomingEvents = mapper.Map<UpcomingEvent>(model);
            ResponseDTO response = upcomingEventsDataFactory.AddDetails(upcomingEvents);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("UpcomingEvents");
        }
        public IActionResult EditUpcomingEvent(int id)
        {
            UpcomingEvent evnt = upcomingEventsDataFactory.GetDetailsById(id);
            UpcomingEventsViewModel viewModel = mapper.Map<UpcomingEventsViewModel>(evnt);
            viewModel.EventTitles = eventTitlesDataFactory.GetEventTitles().Select(x => new SelectListItem() { Value = x.EventTitle, Text = x.EventTitle }).ToList();
            viewModel.EventTitles.Insert(0, new SelectListItem() { Text = " Select Event Title ", Value = "0" });
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult EditUpcomingEvent(UpcomingEventsViewModel viewModel)
        {

            UpcomingEvent upcoming = mapper.Map<UpcomingEvent>(viewModel);
            ResponseDTO response = upcomingEventsDataFactory.UpdateDetails(upcoming);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("UpcomingEvents");
        }

        public IActionResult DeleteUpcomingEvent(int id)
        {
            upcomingEventsDataFactory.DeleteById(id);
            _toastNotification.AddSuccessToastMessage($"Event details removed successfully.");
            return RedirectToAction("UpcomingEvents");
        }



        public IActionResult EventTitles()
        {
            EventTitlesViewModel viewModel = new EventTitlesViewModel()
            {
                EventTitles = eventTitlesDataFactory.GetEventTitles()
            };
            return View(viewModel);
        }

        public IActionResult AddEventTitle(EventTitlesViewModel titlesViewModel)
        {
            EventTitles eventTitles = mapper.Map<EventTitles>(titlesViewModel);
            ResponseDTO response = eventTitlesDataFactory.AddDetails(eventTitles);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("EventTitles");
        }

        public IActionResult EditEventTitle(int id)
        {
            EventTitles eventtitle = eventTitlesDataFactory.GetEventTitleGetById(id);
            EventTitlesViewModel model = mapper.Map<EventTitlesViewModel>(eventtitle);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditEventTitle(EventTitlesViewModel titlesViewModel)
        {
            EventTitles title = mapper.Map<EventTitles>(titlesViewModel);
            ResponseDTO response = eventTitlesDataFactory.UpdateDetails(title);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("EventTitles");
        }

        public IActionResult DeleteEventTitle(int id)
        {
          ResponseDTO response=  eventTitlesDataFactory.DeleteEventTitle(id);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("EventTitles");
        }

        public IActionResult MemberMeetings()
        {
            MemberMeetingsViewModel viewModel = new MemberMeetingsViewModel()
            {
                MemberMeetings = meetingsDataFactory.GetMemberMeetings(),
                MeetingDate = DateTime.Now
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AddMemberMeeting(MemberMeetingsViewModel model)
        {
            MemberMeetings meetings = mapper.Map<MemberMeetings>(model);
            ResponseDTO response = meetingsDataFactory.AddMeetingDetails(meetings);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("MemberMeetings");
        }

        public IActionResult EditMeeting(int Id)
        {
            MemberMeetings meetings = meetingsDataFactory.GetMemberMeetingById(Id);
            MemberMeetingsViewModel model = mapper.Map<MemberMeetingsViewModel>(meetings);

            return View(model);
        }

        [HttpPost]
        public IActionResult EditMeeting(MemberMeetingsViewModel viewModel)
        {
            MemberMeetings meetings = mapper.Map<MemberMeetings>(viewModel);
            ResponseDTO response = meetingsDataFactory.UpdateMeetingDetails(meetings);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("MemberMeetings");
        }

        public IActionResult DeleteMeeting(int Id)
        {
            ResponseDTO response = meetingsDataFactory.DeleteMeetingDetails(Id);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("MemberMeetings");
        }

        public IActionResult ManageBannerAds()
        {
            return View();
        }

        public IActionResult ManageSubscriptions()
        {
            return View();
        }
    }
}
