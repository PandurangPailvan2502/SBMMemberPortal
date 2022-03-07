using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberBusinessViewModel
    {
        public int BusinessId { get; set; }
        public int MemberId { get; set; }
        public string BusinessTitle { get; set; }
        public string BusinessIndustry { get; set; }
        public string CompanyLocation { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string ProductsServices { get; set; }
        public string Qualification { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyEmail { get; set; }
    }
}
