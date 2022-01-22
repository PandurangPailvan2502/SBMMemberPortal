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
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MarritalStatus { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string LandMark { get; set; }
        public string City { get; set; }
        public string Taluka { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }

    }
}
