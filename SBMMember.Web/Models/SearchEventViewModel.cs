using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class SearchEventViewModel
    {
        public string EventYear { get; set; }
        public string EventTitle { get; set; }
        public List<EventInfo> Events { get; set; }
    }
}
