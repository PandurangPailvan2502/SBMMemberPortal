using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Controllers
{
    [Route("kb")]
    public class KnowledgebaseController : Controller
    {
        public IActionResult Index([FromQuery] string lang ,string _category)
        {
            
            return Content(
                $"category={_category}\n" +
                $"language={lang}\n");
        }
    }
}
