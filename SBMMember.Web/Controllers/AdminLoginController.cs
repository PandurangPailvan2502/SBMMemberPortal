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
    public class AdminLoginController : Controller
    {
        private readonly IAdminUsersDataFactory adminUsersDataFactory;
        public AdminLoginController(IAdminUsersDataFactory _dataFactory)
        {
            adminUsersDataFactory = _dataFactory;
        }
      
        
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(AdminLoginViewModel model)
        {
            AdminUsers admin = adminUsersDataFactory.GetAdminUserByMobile(model.Mobile);
            if (admin.MobileNo == model.Mobile.Trim() && model.Password == admin.Password.Trim())
            {
                //Create the identity for the user  
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,$"{admin.FirstName} {admin.LastName}"),
               
                    new Claim(ClaimTypes.PrimarySid,Convert.ToString( admin.MobileNo))
                   
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("AdminHome", "AdminDashboard");
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
            return RedirectToAction("AdminLogin");
        }
    }
}
