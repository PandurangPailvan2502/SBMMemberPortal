using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class CandidateSearchResponse
    {
      
            public int candidateId { get; set; }
            public int userId { get; set; }
            public string fullName { get; set; }
            public string birthdate { get; set; }
            public string education { get; set; }
            public string profileImage { get; set; }
            public int age { get; set; }
            public string birthHrs { get; set; }
            public string birthMins { get; set; }
            public object birthHrsForKundali { get; set; }
            public object birthMinsForKundali { get; set; }
            public string birthPlace { get; set; }
            public bool isBookMarked { get; set; }
            public bool isInviteSent { get; set; }
        
    }
}
