using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class OrderModel
    {
        public decimal orderAmount { get; set; }
        public decimal orderAmountInSubUnits
        {
            get
            {
                return orderAmount * 100;

            }
        }
        public string Currency { get; set; }
        public int Payment_Capture { get; set; }
        public Dictionary<string, string> Notes { get; set; }
    }
}
