using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;

namespace SBMMember.Data.DataFactory
{
   public class AdminUsersDataFactory: IAdminUsersDataFactory
    {
        private readonly SBMMemberDBContext adminDBContext;
        private readonly ILogger<AdminUsersDataFactory> Logger;
        public AdminUsersDataFactory(SBMMemberDBContext dBContext, ILogger<AdminUsersDataFactory> _Logger)
        {
            adminDBContext = dBContext;
            Logger = _Logger;
        }

        public AdminUsers GetAdminUserByMobile(string mobile )
        {
            return adminDBContext.AdminUsers.Where(x => x.MobileNo == mobile && x.Status == "Active").FirstOrDefault();
        }

        public List<AdminUsers> GetAdminUsers()
        {
            return adminDBContext.AdminUsers.Where(x=>x.Status=="Active").ToList();
        }
        public AdminUsers GetUserById(int Id)
        {
            return adminDBContext.AdminUsers.Where(x => x.Id == Id && x.Status == "Active").FirstOrDefault();
        }

        public ResponseDTO AddSuperUserDetails(AdminUsers admin)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                admin.Status = "Active";
                admin.Createdate = DateTime.Now;
                adminDBContext.AdminUsers.Add(admin);
                affectedRows = adminDBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Admin User details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding Admin User details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Admin User details save operation failed."
                };
            }

            return responseDTO;
        }

        public ResponseDTO UpdateDetails(AdminUsers admin)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var adminUser = adminDBContext.AdminUsers.Where(x => x.Id == admin.Id && x.Status == "Active").FirstOrDefault();
                adminUser.FirstName = admin.FirstName;
                adminUser.LastName = admin.LastName;
                adminUser.MobileNo = admin.MobileNo;
                adminUser.Password = admin.Password;
                

                int affectedRows = 0;

                affectedRows = adminDBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Admin user details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating admin user details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Admin user details save operation failed."
                };
            }

            return responseDTO;
        }
        public ResponseDTO Delete(int Id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var adminUser = adminDBContext.AdminUsers.Where(x => x.Id ==Id && x.Status == "Active").FirstOrDefault();
                adminUser.Status = "InActive";
               


                int affectedRows = 0;

                affectedRows = adminDBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Admin user details deleted successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while deleting admin user details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Admin user details delete operation failed."
                };
            }

            return responseDTO;
        }

    }

    public interface IAdminUsersDataFactory
    {
        AdminUsers GetAdminUserByMobile(string mobile);
        List<AdminUsers> GetAdminUsers();
        AdminUsers GetUserById(int Id);

        ResponseDTO AddSuperUserDetails(AdminUsers admin);
        ResponseDTO UpdateDetails(AdminUsers admin);
        ResponseDTO Delete(int Id);
    }
}
