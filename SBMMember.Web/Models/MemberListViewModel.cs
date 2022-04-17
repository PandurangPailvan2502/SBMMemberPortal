using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Models.MemberSearch;
namespace SBMMember.Web.Models
{
    public class MemberListViewModel
    {
        public List<MemberSearchByDTO> memberList { get; set; }
    }
}
