using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
    public class BannerAds
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string ImageURL { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
