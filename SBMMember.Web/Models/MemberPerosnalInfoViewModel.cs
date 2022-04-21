using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberPerosnalInfoViewModel
    {
        public int MemberId { get; set; }
        public string ActiveTab { get; set; }
        public bool IsNew { get; set; }
        public string  Prefix { get; set; }
        public string  PrefixM { get; set; }
        public string  FirstName { get; set; }
        public string  FirstNameM { get; set; }
        public string  MiddleName { get; set; }
        public string  MiddleNameM { get; set; }
        public string  LastName { get; set; }
        public string  LastNameM { get; set; }
        public string  Gender { get; set; }
        public string  GenderM { get; set; }
        public string  MaritalStatus { get; set; }
        public string  MaritalStatusM { get; set; }
        public DateTime  BirthDate { get; set; }
        public string  BirthDateM { get; set; }
        public int  Age { get; set; }
        public string  AgeM { get; set; }
        public string  BloodGroup { get; set; }
        public string  BloodGroupM { get; set; }
        public string  Address { get; set; }
        public string  AddressM { get; set; }
        public string  Area { get; set; }
        public string  AreaM { get; set; }
        public string  SubArea { get; set; }
        public string  SubAreaM { get; set; }
        public string  LandMark { get; set; }
        public string  LandMarkM { get; set; }
        public string  City { get; set; }
        public string  CityM { get; set; }
        public string  Taluka { get; set; }
        public string  TalukaM { get; set; }
        public string  District { get; set; }
        public string  DistrictM { get; set; }
        public string  State { get; set; }
        public string  StateM { get; set; }
        public int  Pincode { get; set; }
        public string  PincodeM { get; set; }

    }
}
