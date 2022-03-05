using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models.MemberSearchModels
{
    public class MemberSearchByNativeViewModel
    {
        public string NativePlace { get; set; }
        public string NativePlaceTaluka { get; set; }
        public string NativePlaceDistrict { get; set; }
        public string MemberId { get; set; }
        public List<MemberSearchResponseViewModel> MemberList { get; set; }
    }
}
