using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using SBMMember.Models;
using NToastNotify;
namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        private readonly IToastNotification toastNotification;
        public BannerController(IToastNotification toast)
        {
            toastNotification = toast;
        }
        public IActionResult ManageBanners()
        {
            BannerViewModel viewModel = new BannerViewModel()
            {
                Banners = BannerHelper.GetBanners()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult UploadBanner(BannerViewModel bannerViewModel)
        {
            Stream fs = bannerViewModel.BannerFile.OpenReadStream();
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((Int32)fs.Length);

           ResponseDTO responsedto= BannerHelper.UploadBanner(bytes, bannerViewModel.Heading, bannerViewModel.BannerFile.FileName);
            if (responsedto.Result == "Success")
            {
                toastNotification.AddSuccessToastMessage(responsedto.Message);
            }
            else
                toastNotification.AddErrorToastMessage(responsedto.Message);
            return RedirectToAction("ManageBanners");
        }
        public IActionResult ViewBanner()
        {
            return RedirectToAction("ManageBanners");
        }

        public IActionResult DeletBanner(int id)
        {
            ResponseDTO responsedto = BannerHelper.RemoveBanner(id);
            if (responsedto.Result == "Success")
            {
                toastNotification.AddSuccessToastMessage(responsedto.Message);
            }
            else
                toastNotification.AddErrorToastMessage(responsedto.Message);
            return RedirectToAction("ManageBanners");
        }
    }
}
