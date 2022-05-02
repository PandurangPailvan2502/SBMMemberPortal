using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class EventViewModel
    {
        public int EventId { get; set; }

        public string EventName { get; set; }
        public string EventNameM { get; set; }
        public string EventDescription { get; set; }
        public string EventDescriptionM { get; set; }
        public string EventYear { get; set; }
        [Display(Name = "File")]
        public List<IFormFile> FormFiles { get; set; }
        public List<EventInfo> EventInfos { get; set; }
        public List<SelectListItem> EventTitles { get; set; }
        public List<SelectListItem> EventYears { get; set; }
        public List<EventGallery> eventGalleries { get; set; }
    }
}
