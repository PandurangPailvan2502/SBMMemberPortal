using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class LoginViewModel
    {
        public string MobileNumber { get; set; }
        public string MaskedMobileNumber { get; set; }
        public string Password { get; set; }
        public string MPIN { get; set; }
        public string SentOTP { get; set; }
        public bool IsOTPVerified { get; set; }
        public bool IsOTPMismatch { get; set; }
        public string OTPInput { get; set; }
    }
}
