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
using SBMMember.Data.DataFactory;
using Microsoft.AspNetCore.Hosting;

namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        private readonly IToastNotification toastNotification;
        private readonly IBannerAdsDataFactory bannerAdsDataFactory;
        private IWebHostEnvironment Environment;
        public BannerController(IToastNotification toast, IBannerAdsDataFactory factory, IWebHostEnvironment hostEnvironment)
        {
            toastNotification = toast;
            bannerAdsDataFactory = factory;
            Environment = hostEnvironment;
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

            ResponseDTO responsedto = BannerHelper.UploadBanner(bytes, bannerViewModel.Heading, bannerViewModel.BannerFile.FileName);
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

        //Member Portal Banner adds started
        public IActionResult BannerAds()
        {
            BannerViewModel viewModel = new BannerViewModel()
            {
                bannerAds = bannerAdsDataFactory.GetBannerAds()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AddBanner(BannerViewModel viewModel)
        {
            string path = Path.Combine(this.Environment.WebRootPath, "Banners");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Path.GetFileName(viewModel.BannerFile.FileName);
            string fullPath = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                viewModel.BannerFile.CopyTo(stream);

            }

            BannerAds banner = new BannerAds()
            {
                Heading = viewModel.Heading,
                ImageURL = $"~/Banners/{viewModel.BannerFile.FileName}",
            };

            ResponseDTO response = bannerAdsDataFactory.AddBannerAdsDetails(banner);
            if (response.Result == "Success")
            {
                toastNotification.AddSuccessToastMessage(response.Message);
            }
            else
                toastNotification.AddErrorToastMessage(response.Message);


            return RedirectToAction("BannerAds");
        }

        public IActionResult EditBanneradvt(int id)
        {
            BannerAds banner = bannerAdsDataFactory.GetBanerAdsById(id);
            BannerViewModel viewModel = new BannerViewModel();
            viewModel.Id = banner.Id;
            viewModel.Heading = banner.Heading;
            viewModel.ImageURL = banner.ImageURL;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult EditBanneradvt(BannerViewModel model)
        {
            BannerAds banner = new BannerAds();
            if (model.BannerFile != null)
            {
                string path = Path.Combine(this.Environment.WebRootPath, "Banners");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = Path.GetFileName(model.BannerFile.FileName);
                string fullPath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    model.BannerFile.CopyTo(stream);

                }

                banner.Id= model.Id;
                banner.Heading = model.Heading;
                banner.ImageURL = $"~/Banners/{model.BannerFile.FileName}";

            }
            else
            {
                banner.Id = model.Id;
                banner.Heading = model.Heading;
                banner.ImageURL = model.ImageURL;
            }
            ResponseDTO response = bannerAdsDataFactory.UpdateBannerAdsDetails(banner);
            if (response.Result == "Success")
            {
                toastNotification.AddSuccessToastMessage(response.Message);
            }
            else
                toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("BannerAds");
        }
        public IActionResult DeletBannerAdvt(int id)
        {
            ResponseDTO response = bannerAdsDataFactory.DeleteBannerAdsDetails(id);
            if (response.Result == "Success")
            {
                toastNotification.AddSuccessToastMessage(response.Message);
            }
            else
                toastNotification.AddErrorToastMessage(response.Message);

            return RedirectToAction("BannerAds");
        }
    }
}
