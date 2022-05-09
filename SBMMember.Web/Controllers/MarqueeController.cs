using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Data.DataFactory;
using SBMMember.Web.Models;
using Microsoft.AspNetCore.Authorization;
using SBMMember.Models;
using AutoMapper;
using NToastNotify;
namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class MarqueeController : Controller
    {
        private readonly IMarqueeDataFactory marqueeDataFactory;
        private readonly IMapper Mapper;
        private readonly IToastNotification _toastNotification;
        public MarqueeController(IMarqueeDataFactory dataFactory, IMapper mapper, IToastNotification toast)
        {
            marqueeDataFactory = dataFactory;
            Mapper = mapper;
            _toastNotification = toast;
        }
        public IActionResult ManaageMarquees()
        {
            MarqueeViewModel model = new MarqueeViewModel()
            {
                marquees = marqueeDataFactory.GetMarquees()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult AddMarquee(MarqueeViewModel model)
        {
            MarqueeText marquee = Mapper.Map<MarqueeText>(model);
            ResponseDTO response = marqueeDataFactory.AddMarqueeDetails(marquee);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("ManaageMarquees");
        }
        public IActionResult EditMarquee(int Id)
        {
            MarqueeText marquee = marqueeDataFactory.GetMarqueeById(Id);
            MarqueeViewModel model = Mapper.Map<MarqueeViewModel>(marquee);

            return View(model);
        }
        [HttpPost]
        public IActionResult EditMarquee(MarqueeViewModel model)
        {
            MarqueeText marquee = Mapper.Map<MarqueeText>(model);
          ResponseDTO response=  marqueeDataFactory.UpdateMarqueetails(marquee);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("ManaageMarquees");
        }
        public IActionResult DeleteMarquee(int Id)
        {
          ResponseDTO response=  marqueeDataFactory.DeleteMarqueeDetails(Id);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("ManaageMarquees");
        }
    }
}
