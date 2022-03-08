﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Models.MemberSearch
{
    public class MemberSearchByDTO
    {
        public int MemberId { get; set; }
        public string MemberAppId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public int? Pincode { get; set; }
        public string District { get; set; }
        public string NativePlace { get; set; }
        public string NativePlaceTaluka { get; set; }
        public string NativePlaceDistrict { get; set; }
        public int? Age { get; set; }
        public string Qualification { get; set; }
        public string? Proffession { get; set; }
        public string MobileNumber { get; set; }
        public string BloodGroup { get; set; }
        public string Gender { get; set; }
    }
}