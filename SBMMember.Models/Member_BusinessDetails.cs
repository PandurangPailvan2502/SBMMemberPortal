using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Models
{
   public class Member_BusinessDetails
    {
		[Key]
		public int BusinessId { get; set; }
		public int MemberId { get; set; }
		public string BusinessTitle { get; set; }
		public string BusinessTitleM { get; set; }
		public string Address { get; set; }
		public string AddressM { get; set; }
		public string NatureOfBusines { get; set; }
		public string Status { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
