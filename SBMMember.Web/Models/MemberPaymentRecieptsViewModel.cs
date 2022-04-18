using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberPaymentRecieptsViewModel
    {
        public int MemberId { get; set; }
        public string ActiveTab { get; set; }
#nullable enable
        public int? LastMemberShipId { get; set; }
        public int? MembershipId { get; set; }
        public string? MemberShipIdM { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? PaymentMode { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? TransactionNo { get; set; }
        public string? PaymentStatus { get; set; }
        public int? ChagesPaid { get; set; }
        public string? RecieptNo { get; set; }
        
    }
}
