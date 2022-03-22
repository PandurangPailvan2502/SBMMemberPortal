using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SBMMember.Data.DataFactory;
using SBMMember.Web.Models;
using SBMMember.Models;
namespace SBMMember.Web.Controllers
{
    public class ManageMembersController : Controller
    {
        private readonly IMemberDataFactory memberDataFactory;
        public ManageMembersController(IMemberDataFactory dataFactory)
        {
            memberDataFactory = dataFactory;
        }
        public IActionResult MemberList()
        {
            return View();
        }
    }
}
