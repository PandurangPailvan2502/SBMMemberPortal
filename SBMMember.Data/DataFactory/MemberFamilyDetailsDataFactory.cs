using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberFamilyDetailsDataFactory : BaseMemberFactory<Member_FamilyDetails>, IMemberFamilyDetailsDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger Logger;
        public MemberFamilyDetailsDataFactory(SBMMemberDBContext memberDBContext,ILogger logger)
        {
            dBContext = memberDBContext;
            Logger = logger;
        }
        public override ResponseDTO AddDetails(Member_FamilyDetails member_FamilyDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                dBContext.Member_FamilyDetails.Add(member_FamilyDetails);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member family details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding member family details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member family details save operation failed."
                };
            }

            return responseDTO;
        }

        public override Member_FamilyDetails GetDetailsByMemberId(int MemberId)
        {
            throw new NotImplementedException();
        }

        public override ResponseDTO UpdateDetails(Member_FamilyDetails member_FamilyDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_FamilyDetails member_Family = dBContext.Member_FamilyDetails.Where(x => x.FamilyDetailsID == member_FamilyDetails.FamilyDetailsID).First();
                member_Family = member_FamilyDetails;
                int affectedRows = 0;
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member family details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member family details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member family details update operation failed."
                };
            }

            return responseDTO;
        }
    }

    public interface IMemberFamilyDetailsDataFactory
    {
        ResponseDTO AddDetails(Member_FamilyDetails member_FamilyDetails);
        ResponseDTO UpdateDetails(Member_FamilyDetails member_FamilyDetails);
    }
}
