using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberEducationEmploymentInfoViewModel
    {
        public int MemberId { get; set; }
        public string Qualification { get; set; }
       
        public string Proffession { get; set; }
       
        public string CompanyName { get; set; }
       
        public string CompanyAddress { get; set; }
       
        public string BusinessName { get; set; }
       
        public string BusinessAddress { get; set; }
       
    }
}
