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
using NToastNotify;
namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class AdminUsersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAdminUsersDataFactory adminUsersDataFactory;
        private readonly IToastNotification _toastNotification;

        public AdminUsersController(IMapper _mapper, IAdminUsersDataFactory dataFactory, IToastNotification toast)
        {
            mapper = _mapper;
            adminUsersDataFactory = dataFactory;
            _toastNotification = toast;
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
            ResponseDTO response = adminUsersDataFactory.AddSuperUserDetails(admin);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("ManageAdminUsers");
        }
        public IActionResult EditAdminUser(int Id)
        {
            AdminUsers admin = adminUsersDataFactory.GetUserById(Id);
            AdminUsersViewModel model = mapper.Map<AdminUsersViewModel>(admin);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditAdminUser(AdminUsersViewModel model)
        {
            AdminUsers admin = mapper.Map<AdminUsers>(model);
            ResponseDTO response = adminUsersDataFactory.UpdateDetails(admin);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("ManageAdminUsers");
        }
        public IActionResult DeleteAdminUser(int Id)
        {
            ResponseDTO response = adminUsersDataFactory.Delete(Id);
            if (response.Result == "Success")
                _toastNotification.AddSuccessToastMessage(response.Message);
            else
                _toastNotification.AddErrorToastMessage(response.Message);
            return RedirectToAction("ManageAdminUsers");
        }
    }
}
