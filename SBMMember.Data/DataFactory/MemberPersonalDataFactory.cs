using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberPersonalDataFactory : IMemberPersonalDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger<MemberPersonalDataFactory> logger;
        public MemberPersonalDataFactory(SBMMemberDBContext memberDBContext, ILogger<MemberPersonalDataFactory> _logger)
        {
            dBContext = memberDBContext;
            logger = _logger;
        }

        public ResponseDTO AddMemberPersonalDetails(Member_PersonalDetails member_Personal)
        {
            ResponseDTO responseDTO=new ResponseDTO();
            try
            {
                int affectedRows = 0;
                dBContext.Member_PersonalDetails.Add(member_Personal);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member personal details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while adding member personal details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member personal details save operation failed."
                };
            }

            return responseDTO;
        }

        public ResponseDTO UpdateMemberPersonalDetails(Member_PersonalDetails member_Personal)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_PersonalDetails personalDetails = dBContext.Member_PersonalDetails.Where(x => x.MemberId == member_Personal.MemberId).First();
                personalDetails = member_Personal;
                int affectedRows = 0;               
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member personal details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while updating member personal details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member personal details update operation failed."
                };
            }

            return responseDTO;
        }

        public Member_PersonalDetails GetMemberPersonalDetailsByMemberId(int memberId)
        {
            Member_PersonalDetails personalDetails=new Member_PersonalDetails();
            try
            {
               personalDetails = dBContext.Member_PersonalDetails.Where(x => x.MemberId ==memberId).First();
               
                
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while Get member personal details by memberId. Exception:{ex.Message}");
                
            }

            return personalDetails;
        }
    }

    public interface IMemberPersonalDataFactory
    {
        ResponseDTO AddMemberPersonalDetails(Member_PersonalDetails member_Personal);
        ResponseDTO UpdateMemberPersonalDetails(Member_PersonalDetails member_Personal);
        Member_PersonalDetails GetMemberPersonalDetailsByMemberId(int memberId);
    }

   
}
