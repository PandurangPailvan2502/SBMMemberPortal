using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class EventAdsSearchViewModel
    {
        public string EventYear { get; set; }
        public List<EventAds> EventAds { get; set; }
    }
}
