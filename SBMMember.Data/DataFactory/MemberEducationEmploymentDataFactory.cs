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
        private readonly ILogger<MemberEducationEmploymentDataFactory> Logger;
        public MemberEducationEmploymentDataFactory(SBMMemberDBContext memberDBContext, ILogger<MemberEducationEmploymentDataFactory> _logger)
        {
            dBContext = memberDBContext;
            Logger = _logger;
        }
        public override ResponseDTO AddDetails(Member_EducationEmploymentDetails member_EducationEmployment)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            member_EducationEmployment.QualificationM = TranslationHelper.Translate(member_EducationEmployment.Qualification);
            member_EducationEmployment.ProffessionM = TranslationHelper.Translate(member_EducationEmployment.Proffession);
            member_EducationEmployment.CompanyNameM = TranslationHelper.Translate(member_EducationEmployment.CompanyName);
            member_EducationEmployment.CompanyAddressM = TranslationHelper.Translate(member_EducationEmployment.CompanyAddress);
            member_EducationEmployment.BusinessNameM = TranslationHelper.Translate(member_EducationEmployment.BusinessName);
            member_EducationEmployment.BusinessAddressM = TranslationHelper.Translate(member_EducationEmployment.BusinessAddress);

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
                //member_Education = educationEmploymentDetails;
                member_Education.BusinessAddress = educationEmploymentDetails.BusinessAddress;
                if (educationEmploymentDetails.BusinessAddress != null)
                {
                    member_Education.BusinessAddressM = TranslationHelper.Translate(educationEmploymentDetails.BusinessAddress);
                }
                member_Education.BusinessName = educationEmploymentDetails.BusinessName;
                if (educationEmploymentDetails.BusinessName != null)
                {
                    educationEmploymentDetails.BusinessNameM = TranslationHelper.Translate(educationEmploymentDetails.BusinessName);
                };
                member_Education.CompanyAddress = educationEmploymentDetails.CompanyAddress;
                if (educationEmploymentDetails.CompanyAddress != null)
                {
                    member_Education.CompanyAddressM = TranslationHelper.Translate(educationEmploymentDetails.CompanyAddress);
                }
                member_Education.CompanyName = educationEmploymentDetails.CompanyName;
                if (educationEmploymentDetails.CompanyName != null)
                {
                    member_Education.CompanyNameM = TranslationHelper.Translate(educationEmploymentDetails.CompanyName);
                }
                member_Education.Proffession = educationEmploymentDetails.Proffession;
                if (educationEmploymentDetails.Proffession != null)
                {
                    member_Education.ProffessionM = TranslationHelper.Translate(educationEmploymentDetails.Proffession);
                }
                member_Education.Qualification = educationEmploymentDetails.Qualification;
                if (educationEmploymentDetails.Qualification != null)
                {
                    member_Education.QualificationM = TranslationHelper.Translate(educationEmploymentDetails.Qualification);
                }
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
        public ResponseDTO UpdateDetailsNoTranslation(Member_EducationEmploymentDetails educationEmploymentDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_EducationEmploymentDetails member_Education = dBContext.Member_EducationEmploymentDetails.Where(x => x.MemberId == educationEmploymentDetails.MemberId).First();
                //member_Education = educationEmploymentDetails;
                member_Education.BusinessAddress = educationEmploymentDetails.BusinessAddress;
                member_Education.BusinessAddressM = educationEmploymentDetails.BusinessAddressM;
                member_Education.BusinessName = educationEmploymentDetails.BusinessName;
                member_Education.BusinessNameM = educationEmploymentDetails.BusinessNameM;
                member_Education.CompanyAddress = educationEmploymentDetails.CompanyAddress;
                member_Education.CompanyAddressM = educationEmploymentDetails.CompanyAddressM;
                member_Education.CompanyName = educationEmploymentDetails.CompanyName;
                member_Education.CompanyNameM = educationEmploymentDetails.CompanyNameM;
                member_Education.Proffession = educationEmploymentDetails.Proffession;
                member_Education.ProffessionM = educationEmploymentDetails.ProffessionM;
                member_Education.Qualification = educationEmploymentDetails.Qualification;
                member_Education.QualificationM = educationEmploymentDetails.QualificationM;

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
        ResponseDTO UpdateDetailsNoTranslation(Member_EducationEmploymentDetails educationEmploymentDetails);
    }
}
