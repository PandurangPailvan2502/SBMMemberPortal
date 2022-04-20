using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberEducationEmploymentInfoViewModel
    {
        public int MemberId { get; set; }
        public bool IsNew { get; set; }
        public string  Qualification { get; set; }
        public string  QualificationM { get; set; }
        public string  Proffession { get; set; }
        public string  ProffessionM { get; set; }
        public string  CompanyName { get; set; }
        public string  CompanyNameM { get; set; }
        public string  CompanyAddress { get; set; }
        public string  CompanyAddressM { get; set; }
        public string  BusinessName { get; set; }
        public string  BusinessNameM { get; set; }
        public string  BusinessAddress { get; set; }
        public string  BusinessAddressM { get; set; }
        public string ActiveTab { get; set; }

    }
}
