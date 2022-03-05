using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
namespace SBMMember.Web.Controllers
{
    public class MatrimonyController : Controller
    {
        public IActionResult MatrimonySearch()
        {
            return View();
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
    }
}
