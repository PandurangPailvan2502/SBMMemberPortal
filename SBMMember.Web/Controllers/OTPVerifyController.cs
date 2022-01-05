using Microsoft.AspNetCore.Mvc;
using SBMMember.Web.Helper;
using SBMMember.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SBMMember.Web.Controllers
{
    public class OTPVerifyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendOTP(string mobile)
        {
            //string mobno = mobile;
            string OTP = SMSHelper.GenerateOTP();
            string message = $"Your OTP for SBM Member registration is {OTP}";
            //SMSHelper.SendSMS(mobile,message);
            string maskedMobile = mobile.Substring(mobile.Length - 4).PadLeft(mobile.Length, 'x');
            ViewBag.MobileNumber = mobile;
            ViewBag.MaskedMobile = maskedMobile;
            ViewBag.SentOTP = OTP;
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                MobileNumber = mobile,
                MaskedMobileNumber = maskedMobile,
                SentOTP = "234567"
            };
            return View("VerifyOTP",loginViewModel);
            //return Json(new { status = "success"});
        }

        
        public IActionResult VerifyOTP()
        {

            return View();
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
        public IActionResult RedirectToSetMPIN(LoginViewModel model)
        {

            return RedirectToActionPermanent("SetPasswordAndPin","Member", model);
        }

    }
}
