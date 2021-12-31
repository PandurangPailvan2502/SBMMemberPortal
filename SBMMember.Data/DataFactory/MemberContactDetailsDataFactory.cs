using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberContactDetailsDataFactory : BaseMemberFactory<Member_ContactDetails>, IMemberContactDetailsDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private ILogger Logger;
        public MemberContactDetailsDataFactory(SBMMemberDBContext memberDBContext, ILogger logger)
        {
            dBContext = memberDBContext;
            Logger = logger;
        }

        public override ResponseDTO AddDetails(Member_ContactDetails member_ContactDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                dBContext.Member_ContactDetails.Add(member_ContactDetails);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member contact details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding member contact details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member contact details save operation failed."
                };
            }

            return responseDTO;
        }

        public override Member_ContactDetails GetDetailsByMemberId(int MemberId)
        {
            Member_ContactDetails member_ContactDetails = new Member_ContactDetails();
            try
            {
                member_ContactDetails = dBContext.Member_ContactDetails.Where(x => x.MemberId == MemberId).First();


            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while Get member contact details by memberId. Exception:{ex.Message}");

            }

            return member_ContactDetails;
        }

        public override ResponseDTO UpdateDetails(Member_ContactDetails member_ContactDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_ContactDetails member_Contact = dBContext.Member_ContactDetails.Where(x => x.MemberId == member_ContactDetails.MemberId).First();
                member_Contact = member_ContactDetails;
                int affectedRows = 0;
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member contact details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member contact details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member contact details update operation failed."
                };
            }

            return responseDTO;
        }
    }

    public interface IMemberContactDetailsDataFactory
    {
        ResponseDTO AddDetails(Member_ContactDetails member_ContactDetails);
        Member_ContactDetails GetDetailsByMemberId(int MemberId);
        ResponseDTO UpdateDetails(Member_ContactDetails member_ContactDetails);
    }
}
