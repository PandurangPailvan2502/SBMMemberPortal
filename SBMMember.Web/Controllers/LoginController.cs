using Microsoft.AspNetCore.Mvc;
using SBMMember.Web.Models;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Data.DataFactory;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SBMMember.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IMemberDataFactory memberDataFactory;
        private readonly IMemberFormStatusDataFactory formStatusDataFactory;
        private readonly IMemberPaymentsDataFactory paymentsDataFactory;
        private readonly IMemberPersonalDataFactory personalDataFactory;
        private readonly IMarqueeDataFactory marqueeDataFactory;
        public LoginController(IMemberDataFactory _dataFactory, IMemberFormStatusDataFactory statusDataFactory, IMemberPaymentsDataFactory _paymentsDataFactory, IMemberPersonalDataFactory _personalDataFactory,
             IMarqueeDataFactory _marqueeDataFactory)
        {
            memberDataFactory = _dataFactory;
            formStatusDataFactory = statusDataFactory;
            paymentsDataFactory = _paymentsDataFactory;
            personalDataFactory = _personalDataFactory;
            marqueeDataFactory = _marqueeDataFactory;
        }
        public IActionResult Login()
        {
            List<string> marqueetxt = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();
            string cookieValueForMobile = Request.Cookies["Mobile"];
            string cookieValueForMpin = Request.Cookies["Mpin"];
            LoginViewModel model = new LoginViewModel()
            {
                MarqueeString = string.Join(", ", marqueetxt)
            };
            if (!string.IsNullOrEmpty(cookieValueForMobile) && !string.IsNullOrEmpty(cookieValueForMpin))
            {
                model.MobileNumber = cookieValueForMobile.Trim();
                model.MPIN = cookieValueForMpin.Trim();
                model.RememberMe = true;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            Members member = memberDataFactory.GetDetailsByMemberMobile(model.MobileNumber);
            if (member != null)
            {
                if (member.Mpin != null)
                {
                    if (member.Mobile == model.MobileNumber.Trim() && model.MPIN == member.Mpin.Trim())
                    {
                        var memberFormStatus = formStatusDataFactory.GetDetailsByMemberId(member.MemberId);
                        if (memberFormStatus.FormStatus == "Approved")
                        {
                            var memberPayment = paymentsDataFactory.GetDetailsByMemberId(member.MemberId);
                            var memberPersonal = personalDataFactory.GetMemberPersonalDetailsByMemberId(member.MemberId);
                            //Create the identity for the user  
                            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,$"{memberPersonal.FirstName} {memberPersonal.MiddleName} {memberPersonal.LastName}"),
                    new Claim("MemberId",Convert.ToString(member.MemberId)),
                     new Claim("MemberShipId",Convert.ToString(memberPayment.MembershipId)),
                    new Claim(ClaimTypes.PrimarySid,Convert.ToString( member.MemberId))

                }, CookieAuthenticationDefaults.AuthenticationScheme);

                            var principal = new ClaimsPrincipal(identity);

                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            if (model.RememberMe)
                            {
                                SetCookie("Mobile", member.Mobile, 120);
                                SetCookie("Mpin", member.Mpin, 120);
                            }
                            else
                            {
                                Response.Cookies.Delete("Mobile");
                                Response.Cookies.Delete("Mpin");
                            }

                            return RedirectToAction("Index", "MemberDashboard");
                        }
                        else if (memberFormStatus.FormStatus == "DeActivated")
                        {
                            ViewBag.ErrorOnLogin = "Your Member profilehas been deactivated.";
                        }
                        else
                        {
                            ViewBag.ErrorOnLogin = "Your Member profile is not approved by Admin.";
                        }
                    }

                    else
                    {
                        ViewBag.ErrorOnLogin = "Invalid Mobile number or MPin";
                    }
                }
                else
                {
                    ViewBag.ErrorOnLogin = "Mpin does not exist for provided mobile no. you can set using  forgot password option.";
                }
            }
            else
            {
                ViewBag.ErrorOnLogin = "Member profile does not exist with provide mobile number.";
            }
            List<string> marqueetxt = marqueeDataFactory.GetMarquees().Select(x => x.Marquee).ToList();

            LoginViewModel _model = new LoginViewModel()
            {
                MarqueeString = string.Join(", ", marqueetxt)
            };
            return View(_model);
        }


        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }
    }
}
