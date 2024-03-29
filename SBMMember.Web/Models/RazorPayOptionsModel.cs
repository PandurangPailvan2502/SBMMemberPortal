﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Models
{
    public class RazorPayOptionsModel
    {
        public int MemberId { get; set; }
        public string Key { get; set; }
        public decimal AmountInSubUnits { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoImageUrl { get; set; }
        public string OrderId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileConatct { get; set; }
        public string ProfileEmail { get; set; }
        public Dictionary<string,string> Notes { get; set; }
    }
}
