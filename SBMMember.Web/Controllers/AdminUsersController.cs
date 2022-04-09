using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Models;
using AutoMapper;
using SBMMember.Data.DataFactory;

namespace SBMMember.Web.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAdminUsersDataFactory adminUsersDataFactory;

        public AdminUsersController(IMapper _mapper,IAdminUsersDataFactory dataFactory)
        {
            mapper = _mapper;
            adminUsersDataFactory = dataFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageAdminUsers()
        {
            AdminUsersViewModel model = new AdminUsersViewModel()
            {
                AdminUsers = adminUsersDataFactory.GetAdminUsers()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult AddAdminUser(AdminUsersViewModel model)
        {
            AdminUsers admin = mapper.Map<AdminUsers>(model);

            return RedirectToAction("ManageAdminUsers");
        }
    }
}
