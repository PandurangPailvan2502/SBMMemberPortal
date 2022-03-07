using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Controllers
{
    public class JobDirectoryController : Controller
    {
        public IActionResult JobDirectory()
        {
            return View();
        }
    }
}
