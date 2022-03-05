using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult AdminHome()
        {
            return View();
        }

        public IActionResult JobPost()
        {
            return View();
        }
    }
}
