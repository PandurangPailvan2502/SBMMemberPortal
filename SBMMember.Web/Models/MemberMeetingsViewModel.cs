using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberMeetingsViewModel
    {
        public int Id { get; set; }
        public string MeetingTitle { get; set; }
        public string MeetingURL { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Status { get; set; }
        public List<MemberMeetings> MemberMeetings { get; set; }
    }
}
