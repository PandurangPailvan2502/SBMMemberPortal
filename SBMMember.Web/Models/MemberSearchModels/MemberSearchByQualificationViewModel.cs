using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models.MemberSearchModels
{
    public class MemberSearchByQualificationViewModel
    {
        public int MemberId { get; set; }
        public string Qualification { get; set; }
        public string Proffession { get; set; }
        public List<MemberSearchResponseViewModel> MemberList { get; set; }
    }
}
