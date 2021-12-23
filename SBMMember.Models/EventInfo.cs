using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
    public class EventInfo
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventNameM { get; set; }
        public string EventDecsription { get; set; }
        public string EventDecsriptionM { get; set; }
        public string EventYear { get; set; }
        public string EventYearM { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
