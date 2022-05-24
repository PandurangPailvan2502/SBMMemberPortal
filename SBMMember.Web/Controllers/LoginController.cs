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

namespace SBMMember.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IMemberDataFactory memberDataFactory;
        private readonly IMemberFormStatusDataFactory formStatusDataFactory;
        private readonly IMemberPaymentsDataFactory paymentsDataFactory;
        private readonly IMemberPersonalDataFactory personalDataFactory;
        private readonly IMarqueeDataFactory marqueeDataFactory;
        public LoginController(IMemberDataFactory _dataFactory,IMemberFormStatusDataFactory statusDataFactory, IMemberPaymentsDataFactory _paymentsDataFactory, IMemberPersonalDataFactory _personalDataFactory,
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

            LoginViewModel model = new LoginViewModel()
            {
                MarqueeString = string.Join(", ", marqueetxt)
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            Members member = memberDataFactory.GetDetailsByMemberMobile(model.MobileNumber);
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
    }
}
