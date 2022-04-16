using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Data.DataFactory;
using SBMMember.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace SBMMember.Web.Controllers
{
   /// [Authorize]
    public class MarqueeController : Controller
    {
        private readonly IMarqueeDataFactory marqueeDataFactory;
        public MarqueeController(IMarqueeDataFactory dataFactory)
        {
            marqueeDataFactory = dataFactory;
        }
        public IActionResult ManaageMarquees()
        {
            MarqueeViewModel model = new MarqueeViewModel()
            {
                marquees = marqueeDataFactory.GetMarquees()
            };

            return View(model);
        }
        public IActionResult EditMarquee(int Id)
        {
            return RedirectToAction("ManaageMarquees");
        }
        public IActionResult DeleteMarquee(int Id)
        {
            return RedirectToAction("ManaageMarquees");
        }
    }
}
