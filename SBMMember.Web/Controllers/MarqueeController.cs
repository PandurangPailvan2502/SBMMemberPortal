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

namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class MarqueeController : Controller
    {
        private readonly IMarqueeDataFactory marqueeDataFactory;
       private readonly IMapper Mapper;
        public MarqueeController(IMarqueeDataFactory dataFactory,IMapper mapper)
        {
            marqueeDataFactory = dataFactory;
            Mapper = mapper;
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
            marqueeDataFactory.AddMarqueeDetails(marquee);
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
            marqueeDataFactory.UpdateMarqueetails(marquee);

            return RedirectToAction("ManaageMarquees");
        }
        public IActionResult DeleteMarquee(int Id)
        {
            marqueeDataFactory.DeleteMarqueeDetails(Id);
            return RedirectToAction("ManaageMarquees");
        }
    }
}
