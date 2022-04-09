using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SBMMember.Web.Helper;
namespace SBMMember.Web.Models
{
    public class BannerViewModel
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public IFormFile BannerFile { get; set; }
        public List<BannerClass> Banners { get; set; }
    }
}
