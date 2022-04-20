using System;
using System.ComponentModel.DataAnnotations;

namespace SBMMember.Models
{
    public class Member_PaymentsAndReciepts
    {
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        ///public bool IsNew { get; set; }
#nullable enable
        public int? MembershipId { get; set; }
        public string? MemberShipIdM { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? PaymentMode { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? TransactionNo { get; set; }
        public string? PaymentStatus { get; set; }
        public int? ChagesPaid { get; set; }
        public string? RecieptNo { get; set; }
        public string? Status { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? Updated { get; set; }
    }
}
