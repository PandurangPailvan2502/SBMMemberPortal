using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models.MemberSearchModels
{
    public class MemberBloodGroupSearchViewModel
    {
        public List<SelectListItem> Areas { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public List<MemberSearchResponseViewModel> MemberList { get; set; }
        public string BloodGroup { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
