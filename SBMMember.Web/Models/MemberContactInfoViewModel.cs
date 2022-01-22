using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberContactInfoViewModel
    {
        public int MemberId { get; set; }
        public string NativePlace { get; set; }
        public string NativePlaceTaluka { get; set; }
        public string NativePlaceDist { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string LandLine { get; set; }
        public string EmailId { get; set; }
        public string RelativeName { get; set; }
        public string RelativeAddress { get; set; }
        public string RelativeContact1 { get; set; }
        public string RelativeContact2 { get; set; }

    }
}
