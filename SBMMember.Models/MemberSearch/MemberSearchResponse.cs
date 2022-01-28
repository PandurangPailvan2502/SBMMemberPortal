using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Models.MemberSearch
{
    public class MemberSearchResponse
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Qualification { get; set; }
        public int? Age { get; set; }
        public string MobileNumber { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string NativePlace { get; set; }
        public List<MemberSearchResponse> MemberList { get; set; }
    }
}
