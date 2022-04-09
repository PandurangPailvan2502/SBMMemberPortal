using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Web.Helper;
using Microsoft.AspNetCore.Authorization;

namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        public IActionResult ManageBanners()
        {
            BannerViewModel viewModel = new BannerViewModel()
            {
                Banners = BannerHelper.GetBanners()
            };
            return View(viewModel);
        }

        public IActionResult ViewBanner()
        {
            return RedirectToAction("ManageBanners");
        }

        public IActionResult DeletBanner()
        {
            return RedirectToAction("ManageBanners");
        }
    }
}
