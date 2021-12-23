using System;
using System.ComponentModel.DataAnnotations;

namespace SBMMember.Models
{
    public class JobSearchDetails
    {
        [Key]
        public int JobID { get; set; }
        public string Description { get; set; }
        public string RequiredProficiency { get; set; }
        public string ExperienceLevel { get; set; }
        public string Salary { get; set; }
        public string PostedBy { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime Updated_TS { get; set; }
        public DateTime Created_TS { get; set; }

    }
}
