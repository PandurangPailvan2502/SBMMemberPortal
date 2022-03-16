using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SBMMember.Web.Helper;
using SBMMember.Web.Models;
using System.Diagnostics;
using SBMMember.Data.DataFactory;
using SBMMember.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace SBMMember.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemberDataFactory dataFactory;
        private readonly IConfiguration configuration;
        public HomeController(ILogger<HomeController> logger,IMemberDataFactory memberDataFactory,IConfiguration _config)
        {
            _logger = logger;
            dataFactory = memberDataFactory;
            configuration = _config;
        }

        public IActionResult Index()
        {
            //var data = dataFactory.GetMembers();
            //Members member = new Members()
            //{
            //    FirstName="Pandurang",
            //    MiddleName="Gokul",
            //    LastName="Pailvan",
            //    Mobile="9226718970",
            //    Password="Ishan@2020",
            //    Status="Active",
            //    Createdate=System.DateTime.Now,
            //    UpdateDate=System.DateTime.Now
            //};
            //dataFactory.AddMember(member);
            //string otp = SMSHelper.GenerateOTP();
            //SMSHelper.SendSMS();
            return View();
        }
        public IActionResult MemberHome()
        {


            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                Banners = BannerHelper.GetBanners()
            };
            //int subcharge =Convert.ToInt32(configuration.GetSection("SubscriptionCharges").Value);
            return View(viewModel);
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
