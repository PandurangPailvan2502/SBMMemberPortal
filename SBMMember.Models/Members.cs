using System;
using System.ComponentModel.DataAnnotations;

namespace SBMMember.Models
{
    public class Members
    {
        [Key]
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Status  { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
