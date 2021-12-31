using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberEducationEmploymentDataFactory : BaseMemberFactory<Member_EducationEmploymentDetails>, IMemberEducationEmploymentDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger Logger;
        public MemberEducationEmploymentDataFactory(SBMMemberDBContext memberDBContext,ILogger _logger)
        {
            dBContext = memberDBContext;
            Logger = _logger;
        }
        public override ResponseDTO AddDetails(Member_EducationEmploymentDetails member_EducationEmployment)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                dBContext.Member_EducationEmploymentDetails.Add(member_EducationEmployment);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member education/employment details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding member education and emplyment details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member education and employment details save operation failed."
                };
            }

            return responseDTO;
        }

        public override Member_EducationEmploymentDetails GetDetailsByMemberId(int MemberId)
        {
            Member_EducationEmploymentDetails employmentDetails = new Member_EducationEmploymentDetails();
            try
            {
                employmentDetails = dBContext.Member_EducationEmploymentDetails.Where(x => x.MemberId == MemberId).First();


            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while Get member education/employment details by memberId. Exception:{ex.Message}");

            }

            return employmentDetails;
        }

        public override ResponseDTO UpdateDetails(Member_EducationEmploymentDetails educationEmploymentDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_EducationEmploymentDetails member_Education = dBContext.Member_EducationEmploymentDetails.Where(x => x.MemberId == educationEmploymentDetails.MemberId).First();
                member_Education = educationEmploymentDetails;
                int affectedRows = 0;
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member education and employment details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member education and employment details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member education and employment details update operation failed."
                };
            }

            return responseDTO;
        }
    }

    public interface IMemberEducationEmploymentDataFactory
    {
        ResponseDTO AddDetails(Member_EducationEmploymentDetails member_EducationEmployment);
        Member_EducationEmploymentDetails GetDetailsByMemberId(int MemberId);
        ResponseDTO UpdateDetails(Member_EducationEmploymentDetails educationEmploymentDetails);
    }
}
