using SBMMember.Models;
using SBMMember.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class BannerAndMarqueeViewModel
    {
        public List<BannerClass> Banners { get; set; }
        public string MarqueeString { get; set; }
        public int NotificationCount { get; set; }
        public int NewMemberCount { get; set; }
        public int RecentJobCount { get; set; }
        public MemberMeetings MemberMeeting { get; set; }
        public List<UpcomingEvent> EventInfos { get; set; }
    }
}
