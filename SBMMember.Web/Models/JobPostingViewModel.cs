using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class JobPostingViewModel
    {
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
        public List<JobPostingViewModel> JobPostings { get; set; }
    }
}
