using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Data.DataFactory;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberDataFactory memberDataFactory;
        private readonly ILogger Logger;
        public MemberController(IMemberDataFactory dataFactory,ILogger<MemberController> logger)
        {
            Logger = logger;
            memberDataFactory = dataFactory;
        }
        public IActionResult SetPasswordAndPin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetPasswordAndPin(LoginViewModel model)
        {
            Members members = new Members()
            {
                Mpin = model.MPIN,
                Mobile = model.MobileNumber,
                Status = "Active",
                Createdate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
         var member=   memberDataFactory.AddMember(members);
            return View();
        }
    }
}
