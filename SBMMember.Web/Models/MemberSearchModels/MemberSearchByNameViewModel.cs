using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models.MemberSearchModels
{
    public class MemberSearchByNameViewModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MemberId { get; set; }
        public List<MemberSearchResponseViewModel> MemberList { get; set; }
    }
}
