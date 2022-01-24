using System;
using System.ComponentModel.DataAnnotations;

namespace SBMMember.Models
{
    public class Member_FamilyDetails
    {
        [Key]
        public int FamilyDetailsID { get; set; }
        public int MemberID { get; set; }
#nullable enable
        public string? Name { get; set; }
        public string? NameM { get; set; }
        public string? Relation { get; set; }
        public string? RelationM { get; set; }
        public string? Education { get; set; }
        public string? EducationM { get; set; }
        public string? Occupation { get; set; }
        public string? OccupationM { get; set; }
        public DateTime? DOB { get; set; }
        public string? BloodGroup { get; set; }
        public string? BloodGroupM { get; set; }
        public string? FamilyMemberPhoto { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
