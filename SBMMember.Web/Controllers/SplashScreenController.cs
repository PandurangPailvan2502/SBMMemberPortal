using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Controllers
{
    public class SplashScreenController : Controller
    {
        public IActionResult SplashScreen()
        {
            return View();
        }
    }
}
