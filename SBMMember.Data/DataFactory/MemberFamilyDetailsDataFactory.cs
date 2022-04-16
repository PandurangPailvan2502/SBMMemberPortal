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
        private readonly ILogger<MemberFamilyDetailsDataFactory> Logger;
        public MemberFamilyDetailsDataFactory(SBMMemberDBContext memberDBContext,ILogger<MemberFamilyDetailsDataFactory> logger)
        {
            dBContext = memberDBContext;
            Logger = logger;
        }
        public override ResponseDTO AddDetails(Member_FamilyDetails member_FamilyDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();

            member_FamilyDetails.NameM = TranslationHelper.Translate(member_FamilyDetails.Name);
            member_FamilyDetails.RelationM = TranslationHelper.Translate(member_FamilyDetails.Relation);
            member_FamilyDetails.EducationM = TranslationHelper.Translate(member_FamilyDetails.Education);
            member_FamilyDetails.OccupationM = TranslationHelper.Translate(member_FamilyDetails.Occupation);
            member_FamilyDetails.BloodGroupM = TranslationHelper.Translate(member_FamilyDetails.BloodGroup);

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
        public List< Member_FamilyDetails> GetFamilyDetailsByMemberId(int MemberId)
        {
            return dBContext.Member_FamilyDetails.Where(x => x.MemberID == MemberId).ToList();
        }
        public override ResponseDTO UpdateDetails(Member_FamilyDetails member_FamilyDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_FamilyDetails member_Family = dBContext.Member_FamilyDetails.Where(x => x.FamilyDetailsID == member_FamilyDetails.FamilyDetailsID).First();
                //member_Family = member_FamilyDetails;
                member_Family.BloodGroup = member_FamilyDetails.BloodGroup;
                member_Family.BloodGroupM = member_FamilyDetails.BloodGroupM;
                member_Family.DOB = member_FamilyDetails.DOB;
                member_Family.Education = member_FamilyDetails.Education;
                member_Family.EducationM = member_FamilyDetails.EducationM;
                member_Family.Name = member_FamilyDetails.Name;
                member_Family.NameM = member_FamilyDetails.NameM;
                member_Family.Occupation = member_FamilyDetails.Occupation;
                member_Family.OccupationM = member_FamilyDetails.OccupationM;
                member_Family.Relation = member_FamilyDetails.Relation;
                member_Family.RelationM = member_FamilyDetails.RelationM;
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
        Member_FamilyDetails GetDetailsByMemberId(int MemberId);
        List<Member_FamilyDetails> GetFamilyDetailsByMemberId(int MemberId);
    }
}
