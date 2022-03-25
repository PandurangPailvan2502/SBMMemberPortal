using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Models;
namespace SBMMember.Web.Models
{
    public class EventDetailsViewModel
    {
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public string EventYear { get; set; }
        public List<EventGallery> GalleryImages { get; set; }

    }
}
