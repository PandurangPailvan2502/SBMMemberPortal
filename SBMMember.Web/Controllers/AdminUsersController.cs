using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Web.Models;
using SBMMember.Models;
using AutoMapper;
using SBMMember.Data.DataFactory;
using Microsoft.AspNetCore.Authorization;

namespace SBMMember.Web.Controllers
{
   // [Authorize]
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
            adminUsersDataFactory.AddSuperUserDetails(admin);
            return RedirectToAction("ManageAdminUsers");
        }
        public IActionResult EditAdminUser(int Id)
        {
          AdminUsers admin=  adminUsersDataFactory.GetUserById(Id);
            AdminUsersViewModel model = mapper.Map<AdminUsersViewModel>(admin);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditAdminUser(AdminUsersViewModel model)
        {
            AdminUsers admin = mapper.Map<AdminUsers>(model);
            adminUsersDataFactory.UpdateDetails(admin);
            return RedirectToAction("ManageAdminUsers");
        }
        public IActionResult DeleteAdminUser(int Id)
        {
            adminUsersDataFactory.Delete(Id);
            return RedirectToAction("ManageAdminUsers");
        }
    }
}
