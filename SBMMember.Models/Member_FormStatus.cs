using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
   public class Member_FormStatus
    {
        [Key]
        public int MemberId { get; set; }
#nullable enable
        public string? FormStatus { get; set; }
        public DateTime? FormSubmitDate { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string? VerifiedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string? AdminComments { get; set; }
    }
}
