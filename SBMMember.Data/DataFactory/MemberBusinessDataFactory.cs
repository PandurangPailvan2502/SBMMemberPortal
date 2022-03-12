﻿using System;
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
        public List<Member_BusinessDetails> GetAllBusinessDetails()
        {
            return dBContext.Member_BusinessDetails.Where(x=>x.Status=="Active").ToList();
        }
        public override ResponseDTO UpdateDetails(Member_BusinessDetails member_BusinessDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_BusinessDetails member_Business = dBContext.Member_BusinessDetails.Where(x => x.MemberId == member_BusinessDetails.MemberId).First();
                member_Business = member_BusinessDetails;
                int affectedRows = 0;
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member buisness details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member business details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member business details update operation failed."
                };
            }

            return responseDTO;
        }
    }

    public interface IMemberBusinessDataFactory
    {
        ResponseDTO AddDetails(Member_BusinessDetails member_Business);
        Member_BusinessDetails GetDetailsByMemberId(int MemberId);
        ResponseDTO UpdateDetails(Member_BusinessDetails member_BusinessDetails);
        List<Member_BusinessDetails> GetAllBusinessDetails();
    }
}
