using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SBMMember.Web.Helper;
using SBMMember.Web.Models;
using System.Diagnostics;
using SBMMember.Data.DataFactory;
using SBMMember.Models;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using NToastNotify;
namespace SBMMember.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemberDataFactory memberdataFactory;
        private readonly IConfiguration configuration;
        private readonly IUpcomingEventsDataFactory upcomingEventsDataFactory;
        private readonly IMarqueeDataFactory marqueeDataFactory;
        private readonly IToastNotification toastNotification;
        private readonly IBannerAdsDataFactory bannerAdsDataFactory;
        public HomeController(ILogger<HomeController> logger, IMemberDataFactory memberDataFactory, IConfiguration _config, IUpcomingEventsDataFactory _upcomingEventsDataFactory,
            IMarqueeDataFactory _marqueeDataFactory, IToastNotification toast, IBannerAdsDataFactory adsDataFactory)
        {
            _logger = logger;
            memberdataFactory = memberDataFactory;
            configuration = _config;
            upcomingEventsDataFactory = _upcomingEventsDataFactory;
            marqueeDataFactory = _marqueeDataFactory;
            toastNotification = toast;
            bannerAdsDataFactory = adsDataFactory;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult MemberHome()
        {

            List<BannerClass> bannerClasses = bannerAdsDataFactory.GetBannerAds()
                .Select(x => new BannerClass()
                { heading = x.Heading, imageURL = x.ImageURL })
                .ToList();
            List<string> marquess = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();
            string marqueeText = string.Join(",", marquess);
            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                Banners = bannerClasses,
                MarqueeString = marqueeText
            };


            //int subcharge =Convert.ToInt32(configuration.GetSection("SubscriptionCharges").Value);
            return View(viewModel);
        }
        public IActionResult AboutUs()
        {

            return View(GetBannerAndMarqueeModel());
        }
        private BannerAndMarqueeViewModel GetBannerAndMarqueeModel()
        {
            List<string> marquess = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();
            string marqueeText = string.Join(",", marquess);
            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                //Banners = BannerHelper.GetBanners(),
                MarqueeString = marqueeText
            };
            return viewModel;
        }
        public IActionResult ContactUs()
        {
            return View(GetBannerAndMarqueeModel());
        }
        public IActionResult PrivacyPolicy()
        {
            return View(GetBannerAndMarqueeModel());
        }
        public IActionResult UpcomingEvents()
        {
            List<string> marquess = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();
            string marqueeText = string.Join(",", marquess);

            BannerAndMarqueeViewModel viewModel = new BannerAndMarqueeViewModel()
            {
                EventInfos = upcomingEventsDataFactory.GetAll(),
                MarqueeString = marqueeText
            };
            return View(viewModel);
        }
        public IActionResult ForgotMPIN()
        {
            List<string> marquess = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();
            string marqueeText = string.Join(",", marquess);
            LoginViewModel model = new LoginViewModel()
            {
                MarqueeString = marqueeText
            };
            return View(model);
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
            else if (member.Status != "Active")
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
            ResponseDTO response = memberdataFactory.UpdateMPIN(members);
            if (response.Result == "Success")
                toastNotification.AddSuccessToastMessage(response.Message);
            else
                toastNotification.AddErrorToastMessage(response.Message);

            return RedirectToActionPermanent("Login", "Login");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
