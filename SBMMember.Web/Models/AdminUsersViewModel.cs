using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Models;
namespace SBMMember.Web.Models
{
    public class AdminUsersViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public List<AdminUsers> AdminUsers { get; set; }
    }
}
