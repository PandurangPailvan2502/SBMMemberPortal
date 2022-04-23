using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
    public class UpcomingEvent
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTitle { get; set; }
        public string EventDesc { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
