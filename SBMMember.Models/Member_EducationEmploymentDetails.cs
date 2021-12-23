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
        public string JobBusinessName { get; set; }
        public string JobBusinessNameM { get; set; }
        public string JobBusinessAddress { get; set; }
        public string JobBusinessAddressM { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
