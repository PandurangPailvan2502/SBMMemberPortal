using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Models.MemberSearch
{
    public class MemberSearchByDTO
    {
        public int MemberId { get; set; }
        public int? MemberShippId { get; set; }
        public string Prefix { get; set; }
        public string PrefixM { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FirstNameM { get; set; }
        public string MiddleNameM { get; set; }
        public string LastNameM { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthdateM { get; set; }
        public string Area { get; set; }
        public string AreaM { get; set; }
        public string City { get; set; }
        public string CityM { get; set; }
        public int? Pincode { get; set; }
        public string District { get; set; }
        public string NativePlace { get; set; }
        public string NativePlaceM { get; set; }
        public string NativePlaceTaluka { get; set; }
        public string NativePlaceDistrict { get; set; }
        public int? Age { get; set; }
        public string Qualification { get; set; }
        public string QualificationM { get; set; }
        public string? Proffession { get; set; }
        public string MobileNumber { get; set; }
        public string BloodGroup { get; set; }
        public string Gender { get; set; }
        public string GenderM { get; set; }
        public string FormStatus { get; set; }
        public string ProfileImage { get; set; }
    }
}
