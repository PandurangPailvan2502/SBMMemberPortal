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
        public LoginController(IMemberDataFactory _dataFactory)
        {
            memberDataFactory = _dataFactory;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            Members member = memberDataFactory.GetDetailsByMemberMobile(model.MobileNumber);
            if (member.Mobile == model.MobileNumber.Trim() && model.MPIN == member.Mpin.Trim())
            {
                //Create the identity for the user  
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,$"{member.FirstName} {member.MiddleName} {member.LastName}"),
                    new Claim("MemberId",Convert.ToString( member.MemberId)),
                    new Claim(ClaimTypes.PrimarySid,Convert.ToString( member.MemberId))
                   
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "MemberDashboard");
            }
            else
            {
                ViewBag.ErrorOnLogin = "Invalid Mobile number or MPin";
            }
            return View();
        }

        
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
