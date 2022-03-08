using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
   
    public class CandidateProfile
    {
        public int candidateId { get; set; }
        public string applicationId { get; set; }
        public int userId { get; set; }
        public string fullName { get; set; }
        public string birthDate { get; set; }
        public string mBirthDate { get; set; }
        public int age { get; set; }
        public string birthName { get; set; }
        public string birthPlace { get; set; }
        public string birthTime { get; set; }
        public string birthHrs { get; set; }
        public string birthMins { get; set; }
        public string bloodGroup { get; set; }
        public string mobileNo { get; set; }
        public string complexion { get; set; }
        public string candidateType { get; set; }
        public string heightInFeet { get; set; }
        public string heightInInches { get; set; }
        public string gotra { get; set; }
        public object mamkul { get; set; }
        public string occupation { get; set; }
        public string qualification { get; set; }
        public string monthlyIncome { get; set; }
        public string company { get; set; }
        public string jobLocation { get; set; }
        public string homeTown { get; set; }
        public string taluka { get; set; }
        public string district { get; set; }
        public string profileImage { get; set; }
        public string educationLevel { get; set; }
        public string expectations { get; set; }
        public string fatherGurdianTitle { get; set; }
        public string nameOfFatherGuardian { get; set; }
        public string parentalAddress { get; set; }
        public string parentalContactNo1 { get; set; }
        public string parentalContactNo2 { get; set; }
        public string parentalEmail { get; set; }
        public string altFatherGurdianTitle { get; set; }
        public string altNameOfFatherGuardian { get; set; }
        public string altParentalAddress { get; set; }
        public string altParentalContactNo1 { get; set; }
        public string altParentalContactNo2 { get; set; }
        public string altParentalEmail { get; set; }
        public string broMarried { get; set; }
        public string broUnmarried { get; set; }
        public string sisMarried { get; set; }
        public string sisUnmarried { get; set; }
        public List<object> userImages { get; set; }
    }
}
