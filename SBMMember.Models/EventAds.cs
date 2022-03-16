using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
   public class EventAds
    {
        public int Id { get; set; }
        public string EventYear { get; set; }
        public string EventTitle { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
