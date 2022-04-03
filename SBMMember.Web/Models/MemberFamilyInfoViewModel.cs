using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberFamilyInfoViewModel
    {
        public int MemberId { get; set; }
        public string  Name { get; set; }
        public string  NameM { get; set; }
        public string  Relation { get; set; }
        public string  RelationM { get; set; }
        public string  Education { get; set; }
        public string  EducationM { get; set; }
        public string  Occupation { get; set; }
        public string  OccupationM { get; set; }
        public DateTime  DOB { get; set; }
        public string  BloodGroup { get; set; }
        public string  BloodGroupM { get; set; }

        public List<MemberFamilyInfoViewModel>? MemberFamilyDetails { get; set; }
    }
}
