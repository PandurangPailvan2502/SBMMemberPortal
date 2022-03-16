using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class EventViewModel
    {
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventYear { get; set; }
        [Display(Name = "File")]
        public List<IFormFile> FormFiles { get; set; }
    }
}
