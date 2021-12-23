using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
    public class EventGallery
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        public string DocType { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
