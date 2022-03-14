using Microsoft.AspNetCore.Mvc;
using SBMMember.Web.Helper;
using SBMMember.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SBMMember.Data.DataFactory;
namespace SBMMember.Web.Controllers
{
    public class OTPVerifyController : Controller
    {
        private readonly IMemberDataFactory MemberDataFactory;
        public OTPVerifyController(IMemberDataFactory dataFactory)
        {
            MemberDataFactory = dataFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendOTP(string mobile)
        {
            var member = MemberDataFactory.GetDetailsByMemberMobile(mobile);
            if(member!=null)
            {
                ViewBag.AlreadyRegistered = $"{mobile} is already registered.Try with another number.";
                return View("Index");
            }
            //string mobno = mobile;
            string OTP = SMSHelper.GenerateOTP();
            string message = $"{OTP} is your OTP for login to Samata Bhratru Mandal (PCMC Pune) Vadhu Var Melava test.com registration portal. Validity for 30 minutes only. Please do not share to anyone else.";
            SMSHelper.SendSMS(mobile,message);
            string maskedMobile = mobile.Substring(mobile.Length - 4).PadLeft(mobile.Length, 'x');
            ViewBag.MobileNumber = mobile;
            ViewBag.MaskedMobile = maskedMobile;
            ViewBag.SentOTP = OTP;
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                MobileNumber = mobile,
                MaskedMobileNumber = maskedMobile,
                SentOTP =OTP
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
            HttpContext.Session.SetString("Mobile", model.MobileNumber);
            return RedirectToAction("SetPasswordAndPin","Member",model);
        }

    }
}
