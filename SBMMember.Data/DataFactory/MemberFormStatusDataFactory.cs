using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberFormStatusDataFactory : BaseMemberFactory<Member_FormStatus>, IMemberFormStatusDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger<MemberFormStatusDataFactory> Logger;
        public MemberFormStatusDataFactory(SBMMemberDBContext memberDBContext,ILogger<MemberFormStatusDataFactory> logger)
        {
            dBContext = memberDBContext;
            Logger = logger;
        }
        public override ResponseDTO AddDetails(Member_FormStatus member_FormStatus)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                dBContext.Member_FormStatuses.Add(member_FormStatus);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member form status details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding member form status details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member form status details save operation failed."
                };
            }

            return responseDTO;
        }

        public override Member_FormStatus GetDetailsByMemberId(int MemberId)
        {
            Member_FormStatus member_Form = new Member_FormStatus();
            try
            {
                member_Form = dBContext.Member_FormStatuses.Where(x => x.MemberId == MemberId).First();


            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while Get member form status details by memberId. Exception:{ex.Message}");

            }

            return member_Form;
        }

        public override ResponseDTO UpdateDetails(Member_FormStatus member_Form)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_FormStatus member_FormStatus = dBContext.Member_FormStatuses.Where(x => x.MemberId == member_Form.MemberId).First();
                member_FormStatus.FormStatus = member_Form.FormStatus;
                member_FormStatus.VerifiedBy = member_Form.VerifiedBy;
                member_FormStatus.VerifiedDate = member_Form.VerifiedDate;
                member_FormStatus.ApprovedBy = member_Form.ApprovedBy;
                member_FormStatus.ApprovedDate = member_Form.ApprovedDate;

                int affectedRows = 0;
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member form status details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member form status details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member form status details update operation failed."
                };
            }

            return responseDTO;
        }
    }

    public interface IMemberFormStatusDataFactory
    {
        ResponseDTO AddDetails(Member_FormStatus member_FormStatus);
        Member_FormStatus GetDetailsByMemberId(int MemberId);
        ResponseDTO UpdateDetails(Member_FormStatus member_Form);
    }

}
