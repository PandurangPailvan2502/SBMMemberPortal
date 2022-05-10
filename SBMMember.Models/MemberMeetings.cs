using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
    public class MemberMeetings
    {
        public int Id { get; set; }
        public string MeetingTitle { get; set; }
        public string MeetingURL { get; set; }
        public DateTime MeetingDate { get; set; }
        
        public string Status { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
