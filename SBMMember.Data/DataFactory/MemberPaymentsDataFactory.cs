﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberPaymentsDataFactory : BaseMemberFactory<Member_PaymentsAndReciepts>, IMemberPaymentsDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger<MemberPaymentsDataFactory> Logger;
        public MemberPaymentsDataFactory(SBMMemberDBContext memberDBContext,ILogger<MemberPaymentsDataFactory> logger)
        {
            dBContext = memberDBContext;
            Logger = logger;
        }
        public override ResponseDTO AddDetails(Member_PaymentsAndReciepts member_Payments)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                dBContext.Member_PaymentsAndReciepts.Add(member_Payments);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member payments details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding member payments details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member payment details save operation failed."
                };
            }

            return responseDTO;
        }

        public override Member_PaymentsAndReciepts GetDetailsByMemberId(int MemberId)
        {
            Member_PaymentsAndReciepts paymentsAndReciepts = new Member_PaymentsAndReciepts();
            try
            {
                paymentsAndReciepts = dBContext.Member_PaymentsAndReciepts.Where(x => x.MemberId == MemberId).First();


            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while Get member education/employment details by memberId. Exception:{ex.Message}");

            }

            return paymentsAndReciepts;
        }
        public int? LastMembershipNo()
        {
            int? lastMembershipNo = 0;
            lastMembershipNo = dBContext.Member_PaymentsAndReciepts.Max(x => x.MembershipId);
            return lastMembershipNo;
               
        }
        public override ResponseDTO UpdateDetails(Member_PaymentsAndReciepts member_Payments)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_PaymentsAndReciepts member_Payments1 = dBContext.Member_PaymentsAndReciepts.Where(x => x.MemberId == member_Payments.MemberId).First();
                member_Payments1.ChagesPaid = member_Payments.ChagesPaid;
                member_Payments1.TransactionDate = member_Payments.TransactionDate;
                member_Payments1.MembershipId = member_Payments.MembershipId;
                member_Payments1.MemberShipIdM =TranslationHelper.Translate(Convert.ToString( member_Payments.MembershipId));
                member_Payments1.RecieptNo = member_Payments.RecieptNo;
                int affectedRows = 0;
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member payment details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member payment details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member payment details update operation failed."
                };
            }

            return responseDTO;
        }
    }

    public interface IMemberPaymentsDataFactory
    {
        ResponseDTO AddDetails(Member_PaymentsAndReciepts member_Payments);
        Member_PaymentsAndReciepts GetDetailsByMemberId(int MemberId);
        ResponseDTO UpdateDetails(Member_PaymentsAndReciepts member_Payments);
        int? LastMembershipNo();
    }
}
