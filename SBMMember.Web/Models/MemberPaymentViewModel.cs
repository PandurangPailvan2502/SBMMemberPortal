using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class MemberPaymentViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int Amount { get; set; }
    }
}
