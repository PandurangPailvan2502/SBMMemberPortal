using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SBMMember.Web.Helper;
using SBMMember.Web.Models;
using System.Diagnostics;
using SBMMember.Data.DataFactory;
using SBMMember.Models;

namespace SBMMember.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemberDataFactory dataFactory;
        public HomeController(ILogger<HomeController> logger,IMemberDataFactory memberDataFactory)
        {
            _logger = logger;
            dataFactory = memberDataFactory;
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
            SMSHelper.SendSMS();
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
