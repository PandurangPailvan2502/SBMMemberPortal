using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SBMMember.Web.Controllers.Enum;

namespace SBMMember.Web.Controllers
{
    public abstract class BaseController : Controller
    {

        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }
    }
    public class Enum
    {
        public enum NotificationType
        {
            error,
            success,
            warning,
            info
        }

    }
}
