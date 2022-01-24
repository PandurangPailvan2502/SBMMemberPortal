using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
    public class Member_ContactDetails
    {
        [Key]
        public int MemberId { get; set; }
#nullable enable
        public string? NativePlace { get; set; }
        public string? NativePlaceM { get; set; }
        public string? NativePlaceTaluka { get; set; }
        public string? NativePlaceTalukaM { get; set; }
        public string? NativePlaceDist { get; set; }
        public string? NativePlaceDistM { get; set; }
        public string? Mobile1 { get; set; }
        public string? Mobile1M { get; set; }
        public string? Mobile2 { get; set; }
        public string? Mobile2M { get; set; }
        public string? LandLine { get; set; }
        public string? LandLineM { get; set; }
        public string? EmailId { get; set; }
        public string? RelativeName { get; set; }
        public string? RelativeNameM { get; set; }
        public string? RelativeAddress { get; set; }
        public string? RelativeAddressM { get; set; }
        public string? RelativeContact1 { get; set; }
        public string? RelativeContact1M { get; set; }
        public string? RelativeContact2 { get; set; }
        public string? RelativeContact2M { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
#nullable disable
    }
}
