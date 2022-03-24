using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Data.DataFactory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IEventDataFactory eventDataFactory;
        private readonly IEventAdsDataFactory eventAdsDataFactory;
        private readonly ILogger<EventsController> logger;

        public EventsController( IEventDataFactory dataFactory,ILogger<EventsController> _logger,IEventAdsDataFactory adsDataFactory)
        {
            eventDataFactory = dataFactory;
            eventAdsDataFactory = adsDataFactory;
            logger = _logger;
        }
        public IActionResult EventDashboard()
        {
            return View();
        }

        public IActionResult SearchEvents()
        {
            SearchEventViewModel model = new SearchEventViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult SearchEvents(SearchEventViewModel model) 
        {
            SearchEventViewModel viewModel = new SearchEventViewModel()
            {
                Events = eventDataFactory.GetEventListByEventYear(model.EventYear)
            };
            return View(viewModel);
        }

        public IActionResult SearchEventAds()
        {
            EventAdsSearchViewModel model = new EventAdsSearchViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult SearchEventAds(EventAdsSearchViewModel model)
        {
            EventAdsSearchViewModel viewModel = new EventAdsSearchViewModel()
            {
                EventAds = eventAdsDataFactory.GetEventAdsByYear(model.EventYear)
        };
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult GetPdf(string filepath)
        {
            //string filePath = "~/file/test.pdf";
            Response.Headers.Add("Content-Disposition", $"inline; filename={filepath.Split('/')[2]}");
            return File(filepath, "application/pdf");
        }

        public IActionResult EventDetails()
        {

            return View();
        }
    }
}
