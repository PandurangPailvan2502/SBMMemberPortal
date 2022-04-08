using Microsoft.AspNetCore.Http;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class EventAdsViewModel
    {
        public int Id { get; set; }
        public string EventTitle { get; set; }
        public string EventYear { get; set; }
        public IFormFile file { get; set; }
        public string FilePath { get; set; }
        public List<EventAds> EventAds { get; set; }
    }
}
