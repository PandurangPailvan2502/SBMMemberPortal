using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Models;
namespace SBMMember.Web.Models
{
    public class MarqueeViewModel
    {
        public int Id { get; set; }
        public string Marquee { get; set; }
        public string Status { get; set; }

        public List<MarqueeText> marquees { get; set; }
    }
}
