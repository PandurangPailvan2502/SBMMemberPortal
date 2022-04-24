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
        private readonly IMemberDataFactory memberdataFactory;
        private readonly IConfiguration configuration;
        private readonly IUpcomingEventsDataFactory upcomingEventsDataFactory;

        public HomeController(ILogger<HomeController> logger, IMemberDataFactory memberDataFactory, IConfiguration _config, IUpcomingEventsDataFactory _upcomingEventsDataFactory)
        {
            _logger = logger;
            memberdataFactory = memberDataFactory;
            configuration = _config;
            upcomingEventsDataFactory = _upcomingEventsDataFactory;
        }

        public IActionResult Index()
        {

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
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        public IActionResult UpcomingEvents()
        {
            UpcomingEventsViewModel viewModel = new UpcomingEventsViewModel()
            {
                EventInfos = upcomingEventsDataFactory.GetAll()
            };
            return View(viewModel);
        }
        public IActionResult ForgotMPIN()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotMPIN(LoginViewModel model)
        {
            Members member = memberdataFactory.GetDetailsByMemberMobile(model.MobileNumber);
            if (member == null)
            {
                ViewBag.AlreadyRegistered = $"Member Profile Does not exit with Provided Mobile No:{model.MobileNumber}";
                return View();
            }
            else if(member.Status!="Active")
            {
                ViewBag.AlreadyRegistered = $"Member Profile is in InActive state with Provided Mobile No:{model.MobileNumber}";
                return View();
            }
            else
            {
                string mobile = model.MobileNumber;
                string OTP = SMSHelper.GenerateOTP();
                string message = $"{OTP} is your OTP for forgot password request. If you have not generated this, please contact Samata Bhratru Mandal (PCMC Pune). Please do not share your OTP with anyone.";
                SMSHelper.SendSMS(model.MobileNumber, message);
                string maskedMobile = mobile.Substring(mobile.Length - 4).PadLeft(mobile.Length, 'x');
                ViewBag.MobileNumber = mobile;
                ViewBag.MaskedMobile = maskedMobile;
                ViewBag.SentOTP = OTP;
                LoginViewModel loginViewModel = new LoginViewModel()
                {
                    MobileNumber = mobile,
                    MaskedMobileNumber = maskedMobile,
                    SentOTP = OTP
                };
                return View("VerifyOTP", loginViewModel);

            }
        }
        [HttpPost]
        public IActionResult VerifyOTP(LoginViewModel model)
        {
            if (model.SentOTP == model.OTPInput)
                model.IsOTPVerified = true;
            else
                model.IsOTPMismatch = true;

            return View("VerifyOTP", model);
        }
        [HttpPost]
        public IActionResult ChangeMPIn(LoginViewModel model)
        {
            Members members = new Members()
            {
                Mobile = model.MobileNumber,
                Mpin = model.MPIN
            };
            memberdataFactory.UpdateMPIN(members);
         return   RedirectToActionPermanent("Login", "Login");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
