using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
    public class Member_EducationEmploymentDetails
    {
        [Key]
        public int MemberId { get; set; }
        public string Qualification { get; set; }
        public string QualificationM { get; set; }
        public string Proffession { get; set; }
        public string ProffessionM { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameM { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyAddressM { get; set; }
        public string BusinessName { get; set; }
        public string BusinessNameM { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessAddressM { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
