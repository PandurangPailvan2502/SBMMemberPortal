using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Models;
namespace SBMMember.Web.Models
{
    public class EventTitlesViewModel
    {
        public int Id { get; set; }
        public string EventTitle { get; set; }
        public string EventTitleM { get; set; }
        public List< EventTitles> EventTitles { get; set; }
    }
}
