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
            member_Personal.PrefixM = TranslationHelper.Translate(member_Personal.Prefix);
            member_Personal.FirstNameM = TranslationHelper.Translate(member_Personal.FirstName);
            member_Personal.MiddleNameM = TranslationHelper.Translate(member_Personal.MiddleName);
            member_Personal.LastNameM = TranslationHelper.Translate(member_Personal.LastName);
            member_Personal.GenderM = TranslationHelper.Translate(member_Personal.Gender);
            member_Personal.MaritalStatusM = TranslationHelper.Translate(member_Personal.MaritalStatus);
            member_Personal.BirthDateM = TranslationHelper.Translate(Convert.ToString( member_Personal.BirthDate));
            member_Personal.AddressM = TranslationHelper.Translate(member_Personal.Address);
            member_Personal.AreaM = TranslationHelper.Translate(member_Personal.Area);
            member_Personal.CityM = TranslationHelper.Translate(member_Personal.City);
            member_Personal.TalukaM = TranslationHelper.Translate(member_Personal.Taluka);
            member_Personal.DistrictM = TranslationHelper.Translate(member_Personal.District);
            member_Personal.StateM = TranslationHelper.Translate(member_Personal.State);
            member_Personal.PincodeM = TranslationHelper.Translate(Convert.ToString( member_Personal.Pincode));

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
                 //personalDetails = member_Personal;
                personalDetails.Address = member_Personal.Address;
                personalDetails.AddressM =TranslationHelper.Translate( member_Personal.Address);
                personalDetails.Age = member_Personal.Age;
                //personalDetails.AgeM =TranslationHelper.Translate( member_Personal.Age);
                personalDetails.Area = member_Personal.Area;
                personalDetails.AreaM =TranslationHelper.Translate( member_Personal.Area);
                personalDetails.BirthDate = member_Personal.BirthDate;
                personalDetails.BirthDateM =TranslationHelper.Translate(Convert.ToString( member_Personal.BirthDate));
                personalDetails.BloodGroup = member_Personal.BloodGroup;
                personalDetails.BloodGroupM = member_Personal.BloodGroupM;
                personalDetails.City = member_Personal.City;
                personalDetails.CityM =TranslationHelper.Translate( member_Personal.City);
                personalDetails.District = member_Personal.District;
                personalDetails.DistrictM =TranslationHelper.Translate( member_Personal.District);
                personalDetails.FirstName = member_Personal.FirstName;
                personalDetails.FirstNameM =TranslationHelper.Translate( member_Personal.FirstName);
                personalDetails.Gender = member_Personal.Gender;
                personalDetails.GenderM =TranslationHelper.Translate( member_Personal.Gender);
                personalDetails.LandMark = member_Personal.LandMark;
                personalDetails.LandMarkM =TranslationHelper.Translate( member_Personal.LandMark);
                personalDetails.LastName = member_Personal.LastName;
                personalDetails.LastNameM =TranslationHelper.Translate( member_Personal.LastName);
                personalDetails.MaritalStatus = member_Personal.MaritalStatus;
                personalDetails.MaritalStatusM =TranslationHelper.Translate( member_Personal.MaritalStatus);
                personalDetails.MiddleName = member_Personal.MiddleName;
                personalDetails.MiddleNameM =TranslationHelper.Translate( member_Personal.MiddleName);
                personalDetails.Pincode = member_Personal.Pincode;
                personalDetails.PincodeM =TranslationHelper.Translate(Convert.ToString( member_Personal.Pincode));
                personalDetails.Prefix = member_Personal.Prefix;
                personalDetails.PrefixM =TranslationHelper.Translate( member_Personal.Prefix);
                personalDetails.State = member_Personal.State;
                personalDetails.StateM =TranslationHelper.Translate( member_Personal.State);
                personalDetails.SubArea = member_Personal.SubArea;
                personalDetails.SubAreaM =TranslationHelper.Translate( member_Personal.SubArea);
                personalDetails.Taluka = member_Personal.Taluka;
                personalDetails.TalukaM =TranslationHelper.Translate( member_Personal.Taluka);


                int affectedRows = 0;
                //dBContext.Update(personalDetails);
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
        public ResponseDTO UpdateMemberPersonalDetailsNoTranslation(Member_PersonalDetails member_Personal)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Member_PersonalDetails personalDetails = dBContext.Member_PersonalDetails.Where(x => x.MemberId == member_Personal.MemberId).First();
                
                personalDetails.Address = member_Personal.Address;
                personalDetails.AddressM = member_Personal.AddressM;
                personalDetails.Age = member_Personal.Age;
                //personalDetails.AgeM =TranslationHelper.Translate( member_Personal.Age);
                personalDetails.Area = member_Personal.Area;
                personalDetails.AreaM = member_Personal.AreaM;
                personalDetails.BirthDate = member_Personal.BirthDate;
                personalDetails.BirthDateM = member_Personal.BirthDateM;
                personalDetails.BloodGroup = member_Personal.BloodGroup;
                personalDetails.BloodGroupM = member_Personal.BloodGroupM;
                personalDetails.City = member_Personal.City;
                personalDetails.CityM = member_Personal.CityM;
                personalDetails.District = member_Personal.District;
                personalDetails.DistrictM = member_Personal.DistrictM;
                personalDetails.FirstName = member_Personal.FirstName;
                personalDetails.FirstNameM = member_Personal.FirstNameM;
                personalDetails.Gender = member_Personal.Gender;
                personalDetails.GenderM = member_Personal.GenderM;
                personalDetails.LandMark = member_Personal.LandMark;
                personalDetails.LandMarkM = member_Personal.LandMarkM;
                personalDetails.LastName = member_Personal.LastName;
                personalDetails.LastNameM = member_Personal.LastNameM;
                personalDetails.MaritalStatus = member_Personal.MaritalStatus;
                personalDetails.MaritalStatusM =member_Personal.MaritalStatusM;
                personalDetails.MiddleName = member_Personal.MiddleName;
                personalDetails.MiddleNameM =member_Personal.MiddleNameM;
                personalDetails.Pincode = member_Personal.Pincode;
                personalDetails.PincodeM = member_Personal.PincodeM;
                personalDetails.Prefix = member_Personal.Prefix;
                personalDetails.PrefixM = member_Personal.PrefixM;
                personalDetails.State = member_Personal.State;
                personalDetails.StateM = member_Personal.StateM;
                personalDetails.SubArea = member_Personal.SubArea;
                personalDetails.SubAreaM = member_Personal.SubAreaM;
                personalDetails.Taluka = member_Personal.Taluka;
                personalDetails.TalukaM = member_Personal.TalukaM;


                int affectedRows = 0;
                //dBContext.Update(personalDetails);
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

       public List<string> GetDistinctAreas()
        {
            return dBContext.Member_PersonalDetails.Where(s => !string.IsNullOrWhiteSpace(s.Area)).Select(x => x.Area).Distinct().ToList();
        }

        public List<string> GetDistinctCities()
        {
            return dBContext.Member_PersonalDetails.Where(s => !string.IsNullOrWhiteSpace(s.City)).Select(x => x.City).Distinct().ToList();
        }
    }

    public interface IMemberPersonalDataFactory
    {
        ResponseDTO AddMemberPersonalDetails(Member_PersonalDetails member_Personal);
        ResponseDTO UpdateMemberPersonalDetails(Member_PersonalDetails member_Personal);
        Member_PersonalDetails GetMemberPersonalDetailsByMemberId(int memberId);
        List<string> GetDistinctAreas();
        List<string> GetDistinctCities();
        ResponseDTO UpdateMemberPersonalDetailsNoTranslation(Member_PersonalDetails member_Personal);
    }

   
}
