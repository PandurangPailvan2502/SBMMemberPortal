using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models.MemberSearchModels
{
    public class MemberSearchByAreaViewModel
    {
        public string Area { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int Pincode { get; set; }
        public List<MemberSearchResponseViewModel> MemberList { get; set; }
    }
}
