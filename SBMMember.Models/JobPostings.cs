using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
   public class JobPostings
    {
        [Key]
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string PositionFor { get; set; }
        public string Age { get; set; }
        public string CompanyName { get; set; }
        public string JobLocation { get; set; }
        public string Qualification { get; set; }
        public string SalaryBand { get; set; }
        public string Experiance { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyEmail { get; set; }
        public DateTime PostedOn { get; set; }
        public string Status { get; set; }
    }
}
