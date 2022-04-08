using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBMMember.Models;

namespace SBMMember.Data.DataFactory
{
   public class AdminUsersDataFactory: IAdminUsersDataFactory
    {
        private readonly SBMMemberDBContext adminDBContext;
        public AdminUsersDataFactory(SBMMemberDBContext dBContext)
        {
            adminDBContext = dBContext;
        }

        public AdminUsers GetAdminUserByMobile(string mobile )
        {
            return adminDBContext.AdminUsers.Where(x => x.MobileNo == mobile && x.Status == "Active").FirstOrDefault();
        }
    }

    public interface IAdminUsersDataFactory
    {
        AdminUsers GetAdminUserByMobile(string mobile);
    }
}
