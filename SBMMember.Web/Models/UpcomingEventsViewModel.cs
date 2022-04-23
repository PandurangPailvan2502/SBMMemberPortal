using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class UpcomingEventsViewModel
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTitle { get; set; }
        public string EventDesc { get; set; }
        public List<UpcomingEvent> EventInfos { get; set; }
    }
}
