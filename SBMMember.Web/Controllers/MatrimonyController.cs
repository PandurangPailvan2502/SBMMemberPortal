using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using SBMMember.Data.DataFactory;
namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class MatrimonyController : Controller
    {
        private readonly IBannerAdsDataFactory bannerAdsDataFactory;
        public MatrimonyController(IBannerAdsDataFactory adsDataFactory)
        {
            bannerAdsDataFactory= adsDataFactory;

        }
        public IActionResult MatrimonySearch()
        {
            List<BannerClass> bannerClasses = bannerAdsDataFactory.GetBannerAds().Select(x => new BannerClass() { heading = x.Heading, imageURL = x.ImageURL }).ToList();
            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                //Banners = BannerHelper.GetBanners()
                Banners=bannerClasses
            };
            return View(viewModel);
        }
        public IActionResult VarSearch()
        {
            MatrimonyViewModel viewModel = new MatrimonyViewModel()
            {
                Candidates = Helper.MatrimonyHelper.GetAllVar()
            };
            return View("MatrimonyCandidateList",viewModel);
        }
        public IActionResult VadhuSearch()
        {
            MatrimonyViewModel viewModel = new MatrimonyViewModel()
            {
                Candidates = Helper.MatrimonyHelper.GetAllVadhus()
            };
            return View("MatrimonyCandidateList", viewModel);
        }

        public IActionResult MatrimonyCandidateProfile(string UserIdId)
        {
            MatrimonyViewModel viewModel = new MatrimonyViewModel()
            {
                CandidateProfile = Helper.MatrimonyHelper.GetCandidateProfileByCandidateId(UserIdId)
            };
            return View(viewModel);
        }
    }
}
