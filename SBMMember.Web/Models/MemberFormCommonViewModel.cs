using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberFormCommonViewModel
    {
        public int MemberId { get; set; }
        public int ProfilePercentage { get; set; }
        public IFormFile file { get; set; }
        public MemberPerosnalInfoViewModel MemberPersonalInfo { get; set; }
        public MemberContactInfoViewModel MemberConatctInfo { get; set; }
        public MemberEducationEmploymentInfoViewModel MemberEducationEmploymentInfo { get; set; }
        public MemberFamilyInfoViewModel MemberFamilyInfo { get; set; }
        public MemberPaymentRecieptsViewModel MemberPaymentInfo { get; set; }
    }
}
