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
        private ILogger<MemberContactDetailsDataFactory> logger;
        public MemberContactDetailsDataFactory(SBMMemberDBContext memberDBContext, ILogger<MemberContactDetailsDataFactory> _logger)
        {
            dBContext = memberDBContext;
            logger = _logger;
        }

        public override ResponseDTO AddDetails(Member_ContactDetails member_ContactDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            member_ContactDetails.NativePlaceM = TranslationHelper.Translate(member_ContactDetails.NativePlace);
            member_ContactDetails.NativePlaceTalukaM = TranslationHelper.Translate(member_ContactDetails.NativePlaceTaluka);
            member_ContactDetails.NativePlaceDistM = TranslationHelper.Translate(member_ContactDetails.NativePlaceDist);
            member_ContactDetails.Mobile1M = TranslationHelper.Translate(member_ContactDetails.Mobile1);
            if(member_ContactDetails.Mobile2!=null)
            member_ContactDetails.Mobile2M = TranslationHelper.Translate(member_ContactDetails.Mobile2);
            member_ContactDetails.LandLineM = TranslationHelper.Translate(member_ContactDetails.LandLine);
            member_ContactDetails.RelativeNameM = TranslationHelper.Translate(member_ContactDetails.RelativeName);
            member_ContactDetails.RelativeAddressM = TranslationHelper.Translate(member_ContactDetails.RelativeAddress);
            member_ContactDetails.RelativeContact1M = TranslationHelper.Translate(member_ContactDetails.RelativeContact1);
            if (member_ContactDetails.RelativeContact2 != null)
                member_ContactDetails.RelativeContact2M = TranslationHelper.Translate(member_ContactDetails.RelativeContact2);

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

                logger.LogError($"Error occured while adding member contact details. Exception:{ex.Message}");
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

                logger.LogError($"Error occured while Get member contact details by memberId. Exception:{ex.Message}");

            }

            return member_ContactDetails;
        }

        public override ResponseDTO UpdateDetails(Member_ContactDetails member_ContactDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_ContactDetails member_Contact = dBContext.Member_ContactDetails.Where(x => x.MemberId == member_ContactDetails.MemberId).First();
                //member_Contact = member_ContactDetails;
                member_Contact.EmailId = member_ContactDetails.EmailId;
                member_Contact.LandLine = member_ContactDetails.LandLine;
                member_Contact.LandLineM =TranslationHelper.Translate( member_ContactDetails.LandLine);
                member_Contact.Mobile1 = member_ContactDetails.Mobile1;
                member_Contact.Mobile1M =TranslationHelper.Translate( member_ContactDetails.Mobile1);
                member_Contact.Mobile2 = member_ContactDetails.Mobile2;
                if (member_ContactDetails.Mobile2!=null)
                member_Contact.Mobile2M =TranslationHelper.Translate( member_ContactDetails.Mobile2);
                member_Contact.NativePlace = member_ContactDetails.NativePlace;
                member_Contact.NativePlaceM =TranslationHelper.Translate( member_ContactDetails.NativePlace);
                member_Contact.NativePlaceDist = member_ContactDetails.NativePlaceDist;
                member_Contact.NativePlaceDistM =TranslationHelper.Translate(member_ContactDetails.NativePlaceDist);
                member_Contact.NativePlaceTaluka = member_ContactDetails.NativePlaceTaluka;
                member_Contact.NativePlaceTalukaM =TranslationHelper.Translate(member_ContactDetails.NativePlaceTaluka);
                member_Contact.RelativeAddress = member_ContactDetails.RelativeAddress;
                member_Contact.RelativeAddressM =TranslationHelper.Translate( member_ContactDetails.RelativeAddress);
                member_Contact.RelativeContact1 = member_ContactDetails.RelativeContact1;
                member_Contact.RelativeContact1M =TranslationHelper.Translate( member_ContactDetails.RelativeContact1);
                member_Contact.RelativeContact2 = member_ContactDetails.RelativeContact2;
                if (member_ContactDetails.RelativeContact2 != null)
                    member_Contact.RelativeContact2M =TranslationHelper.Translate( member_ContactDetails.RelativeContact2);
                member_Contact.RelativeName = member_ContactDetails.RelativeName;
                member_Contact.RelativeNameM =TranslationHelper.Translate( member_ContactDetails.RelativeName);
                
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

                logger.LogError($"Error occured while updating member contact details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member contact details update operation failed."
                };
            }

            return responseDTO;
        }
        public  ResponseDTO UpdateDetailsNoTransalation(Member_ContactDetails member_ContactDetails)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_ContactDetails member_Contact = dBContext.Member_ContactDetails.Where(x => x.MemberId == member_ContactDetails.MemberId).First();
                //member_Contact = member_ContactDetails;
                member_Contact.EmailId = member_ContactDetails.EmailId;
                member_Contact.LandLine = member_ContactDetails.LandLine;
                member_Contact.LandLineM = member_ContactDetails.LandLineM;
                member_Contact.Mobile1 = member_ContactDetails.Mobile1;
                member_Contact.Mobile1M = member_ContactDetails.Mobile1M;
                member_Contact.Mobile2 = member_ContactDetails.Mobile2;
                member_Contact.Mobile2M = member_ContactDetails.Mobile2M;
                member_Contact.NativePlace = member_ContactDetails.NativePlace;
                member_Contact.NativePlaceM = member_ContactDetails.NativePlaceM;
                member_Contact.NativePlaceDist = member_ContactDetails.NativePlaceDist;
                member_Contact.NativePlaceDistM = member_ContactDetails.NativePlaceDistM;
                member_Contact.NativePlaceTaluka = member_ContactDetails.NativePlaceTaluka;
                member_Contact.NativePlaceTalukaM = member_ContactDetails.NativePlaceTalukaM;
                member_Contact.RelativeAddress = member_ContactDetails.RelativeAddress;
                member_Contact.RelativeAddressM = member_ContactDetails.RelativeAddressM;
                member_Contact.RelativeContact1 = member_ContactDetails.RelativeContact1;
                member_Contact.RelativeContact1M = member_ContactDetails.RelativeContact1M;
                member_Contact.RelativeContact2 = member_ContactDetails.RelativeContact2;
                member_Contact.RelativeContact2M = member_ContactDetails.RelativeContact2M;
                member_Contact.RelativeName = member_ContactDetails.RelativeName;
                member_Contact.RelativeNameM = member_ContactDetails.RelativeNameM;

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

                logger.LogError($"Error occured while updating member contact details. Exception:{ex.Message}");
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
        ResponseDTO UpdateDetailsNoTransalation(Member_ContactDetails member_ContactDetails);
    }
}
