using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Web.Helper;

namespace SBMMember.Web.Controllers
{
    public class MatrimonyController : Controller
    {
        public IActionResult MatrimonySearch()
        {
            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                Banners = BannerHelper.GetBanners()
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

        public IActionResult MatrimonyCandidateProfile(string CandidateId)
        {
            MatrimonyViewModel viewModel = new MatrimonyViewModel()
            {
                CandidateProfile = Helper.MatrimonyHelper.GetCandidateProfileByCandidateId(CandidateId)
            };
            return View(viewModel);
        }
    }
}
