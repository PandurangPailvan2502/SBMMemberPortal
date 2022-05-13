using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Data.DataFactory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using SBMMember.Web.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IEventDataFactory eventDataFactory;
        private readonly IEventAdsDataFactory eventAdsDataFactory;
        private readonly ILogger<EventsController> logger;
        private readonly IEventTitlesDataFactory eventTitlesDataFactory;
        private readonly IToastNotification toastNotification;
        public EventsController(IEventDataFactory dataFactory, ILogger<EventsController> _logger, IEventAdsDataFactory adsDataFactory, IEventTitlesDataFactory _eventTitlesDataFactory,IToastNotification toast)
        {
            eventDataFactory = dataFactory;
            eventAdsDataFactory = adsDataFactory;
            logger = _logger;
            eventTitlesDataFactory = _eventTitlesDataFactory;
            toastNotification = toast;
        }
        public IActionResult EventDashboard()
        {
            return View();
        }

        public IActionResult SearchEvents()
        {
            SearchEventViewModel model = new SearchEventViewModel()
            {
                EventYears = YearHelper.GetYears(),
                EventTitles = eventTitlesDataFactory.GetEventTitles().Select(x => new SelectListItem() { Value = x.EventTitle, Text = x.EventTitle }).ToList()
            };
            model.EventTitles.Insert(0, new SelectListItem() { Text = " Select Event Title ", Value = "0" });
            model.EventYears.Insert(0, new SelectListItem() { Text = " Select Event Year ", Value = "0" });
            return View(model);
        }
        [HttpPost]
        public IActionResult SearchEvents(SearchEventViewModel model)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (model.EventTitle!="0")
                keyValuePairs.Add("EventTitle", model.EventTitle);
            if (model.EventYear!="0")
                keyValuePairs.Add("EventYear", model.EventYear);
            var events = eventDataFactory.GetEventListByEventParameters(keyValuePairs);
            if(events.Count>0)
            {
                toastNotification.AddInfoToastMessage($"{events.Count} matching result(s) found..");
            }
            else
            {
                toastNotification.AddWarningToastMessage($"No matching results found..");
            }
            SearchEventViewModel viewModel = new SearchEventViewModel()
            {
                Events =events,
                EventYears = YearHelper.GetYears(),
                EventTitles = eventTitlesDataFactory.GetEventTitles().Select(x => new SelectListItem() { Value = x.EventTitle, Text = x.EventTitle }).ToList()
            };
            viewModel.EventTitles.Insert(0, new SelectListItem() { Text = " Select Event Title ", Value = "0" });
            viewModel.EventYears.Insert(0, new SelectListItem() { Text = " Select Event Year ", Value = "0" });
            return View(viewModel);
        }

        public IActionResult SearchEventAds()
        {
            EventAdsSearchViewModel model = new EventAdsSearchViewModel()
            {
                EventYears = YearHelper.GetYears()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult SearchEventAds(EventAdsSearchViewModel model)
        {
            EventAdsSearchViewModel viewModel = new EventAdsSearchViewModel()
            {
                EventAds = eventAdsDataFactory.GetEventAdsByYear(model.EventYear),
                EventYears=YearHelper.GetYears()
            };
            if (viewModel.EventAds.Count > 0)
            {
                toastNotification.AddInfoToastMessage($"{viewModel.EventAds.Count} matching result(s) found..");
            }
            else
            {
                toastNotification.AddWarningToastMessage($"No matching results found..");
            }
            viewModel.EventYears.Insert(0, new SelectListItem() { Text = " Select Event Year ", Value = "0" });
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult GetPdf(string filepath)
        {
            //string filePath = "~/file/test.pdf";
            Response.Headers.Add("Content-Disposition", $"inline; filename={filepath.Split('/')[2]}");
            return File(filepath, "application/pdf");
        }

        public IActionResult EventDetails(string EventId)
        {
            int _eventId = Convert.ToInt32(EventId);
            var eventInfo = eventDataFactory.GetEventInfoByeventId(_eventId);

            EventDetailsViewModel detailsViewModel = new EventDetailsViewModel()
            {
                EventTitle = eventInfo.EventName,
                EventDescription = eventInfo.EventDescription,
                EventYear = eventInfo.EventYear,
                GalleryImages = eventDataFactory.GetGalleryphotosByeventId(_eventId)
            };
            return View(detailsViewModel);
        }
    }
}
