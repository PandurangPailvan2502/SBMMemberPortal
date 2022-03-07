using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult EventDashboard()
        {
            return View();
        }
    }
}
