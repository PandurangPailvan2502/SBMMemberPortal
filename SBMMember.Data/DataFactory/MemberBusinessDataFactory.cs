using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberBusinessDataFactory : BaseMemberFactory<Member_BusinessDetails>, IMemberBusinessDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger<MemberBusinessDataFactory> Logger;
        public MemberBusinessDataFactory(SBMMemberDBContext memberDBContext, ILogger<MemberBusinessDataFactory> logger)
        {
            dBContext = memberDBContext;
            Logger = logger;
        }
        public override ResponseDTO AddDetails(Member_BusinessDetails member_Business)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                member_Business.Status = "Active";
                member_Business.CreateDate = DateTime.Now;
                dBContext.Member_BusinessDetails.Add(member_Business);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member buisness details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding member business details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member business details save operation failed."
                };
            }

            return responseDTO;
        }
        public override ResponseDTO UpdateDetails(Member_BusinessDetails member_Business)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var business = dBContext.Member_BusinessDetails.Where(x => x.BusinessId == member_Business.BusinessId && x.Status == "Active").FirstOrDefault();
                business.Address = member_Business.Address;
                business.BusinessIndustry = member_Business.BusinessIndustry;
                business.BusinessTitle = member_Business.BusinessTitle;
                business.CompanyContact = member_Business.CompanyContact;
                business.CompanyEmail = member_Business.CompanyEmail;
                business.CompanyLocation = member_Business.CompanyLocation;
                business.OwnerName = member_Business.OwnerName;
                business.ProductsServices = member_Business.ProductsServices;
                business.Qualification = member_Business.Qualification;
               
                int affectedRows = 0;              
                
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member buisness details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding member business details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member business details save operation failed."
                };
            }

            return responseDTO;
        }
        public override Member_BusinessDetails GetDetailsByMemberId(int MemberId)
        {
            Member_BusinessDetails member_Business = new Member_BusinessDetails();
            try
            {
                member_Business = dBContext.Member_BusinessDetails.Where(x => x.MemberId == MemberId).First();


            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while Get member business details by memberId. Exception:{ex.Message}");

            }

            return member_Business;
        }
        public Member_BusinessDetails GetDetailsById(int businessId)
        {
            Member_BusinessDetails member_Business = new Member_BusinessDetails();
            try
            {
                member_Business = dBContext.Member_BusinessDetails.Where(x => x.BusinessId == businessId).FirstOrDefault();


            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while Get member business details by memberId. Exception:{ex.Message}");

            }

            return member_Business;
        }
        public ResponseDTO DeleteDetailsById(int businessId)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var business = dBContext.Member_BusinessDetails.Where(x => x.BusinessId ==businessId && x.Status == "Active").FirstOrDefault();
                business.Status ="InActive";
               

                int affectedRows = 0;

                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member buisness details deleted successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while deleting member business details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member business details delete operation failed."
                };
            }

            return responseDTO;
        }
        public List<Member_BusinessDetails> GetAllBusinessDetails()
        {
            return dBContext.Member_BusinessDetails.Where(x=>x.Status=="Active").ToList();
        }
       
    }

    public interface IMemberBusinessDataFactory
    {
        ResponseDTO AddDetails(Member_BusinessDetails member_Business);
        Member_BusinessDetails GetDetailsByMemberId(int MemberId);
        ResponseDTO UpdateDetails(Member_BusinessDetails member_BusinessDetails);
        List<Member_BusinessDetails> GetAllBusinessDetails();
        Member_BusinessDetails GetDetailsById(int businessId);
       ResponseDTO DeleteDetailsById(int businessId);
    }
}
