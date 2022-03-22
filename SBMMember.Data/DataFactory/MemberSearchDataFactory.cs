using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.Extensions.Logging;
using SBMMember.Data.DataFactory;
using SBMMember.Models.MemberSearch;

namespace SBMMember.Data.DataFactory
{
    public class MemberSearchDataFactory : IMemberSearchDataFactory
    {
        private readonly IMemberPersonalDataFactory memberPersonalDataFactory;
        private readonly IMemberContactDetailsDataFactory memberContactDetailsDataFactory;
        private readonly IMemberEducationEmploymentDataFactory memberEducationEmploymentDataFactory;
        private readonly ILogger<MemberSearchDataFactory> logger;
        private readonly SBMMemberDBContext context;
        public MemberSearchDataFactory(ILogger<MemberSearchDataFactory> _logger, IMemberPersonalDataFactory personalDataFactory, IMemberContactDetailsDataFactory contactDetailsDataFactory,
            IMemberEducationEmploymentDataFactory educationEmploymentDataFactory, SBMMemberDBContext dBContext)
        {
            logger = _logger;
            memberPersonalDataFactory = personalDataFactory;
            memberContactDetailsDataFactory = contactDetailsDataFactory;
            memberEducationEmploymentDataFactory = educationEmploymentDataFactory;
            context = dBContext;

        }
        public List<MemberSearchResponse> GetAllDoctors()
        {
            List<MemberSearchByDTO> memberSearches = (from _apppersonal in context.Member_PersonalDetails
                                                      join _appconatct in context.Member_ContactDetails
                                                      on _apppersonal.MemberId equals _appconatct.MemberId
                                                      join _appqualification in context.Member_EducationEmploymentDetails
                                                      on _apppersonal.MemberId equals _appqualification.MemberId
                                                      join _appFormStatus in context.Member_FormStatuses
                                                      on _apppersonal.MemberId equals _appFormStatus.MemberId
                                                      where _appFormStatus.FormStatus == "Approved"
                                                      select new MemberSearchByDTO
                                                      {
                                                          MemberId = _apppersonal.MemberId,
                                                          FirstName = _apppersonal.FirstName,
                                                          FirstNameM=_apppersonal.FirstNameM,
                                                          MiddleName = _apppersonal.MiddleName,
                                                          MiddleNameM=_apppersonal.MiddleNameM,
                                                          LastName = _apppersonal.LastName,
                                                          LastNameM=_apppersonal.LastNameM,
                                                          BirthDate = _apppersonal.BirthDate,
                                                          BirthdateM=_apppersonal.BirthDateM,
                                                          Qualification = _appqualification.Qualification,
                                                          QualificationM=_appqualification.QualificationM,
                                                          Proffession = _appqualification.Proffession,
                                                          MobileNumber = _appconatct.Mobile1,
                                                          NativePlace = _appconatct.NativePlace,
                                                          NativePlaceM=_appconatct.NativePlaceM,
                                                          NativePlaceTaluka = _appconatct.NativePlaceTaluka,
                                                          NativePlaceDistrict = _appconatct.NativePlaceDist,
                                                          Area = _apppersonal.Area,
                                                          AreaM=_apppersonal.AreaM,
                                                          City = _apppersonal.City,
                                                          CityM=_apppersonal.CityM,
                                                          Pincode = _apppersonal.Pincode,
                                                          District = _apppersonal.District != null ? _apppersonal.District : string.Empty,
                                                          Age = _apppersonal.Age,
                                                          BloodGroup=_apppersonal.BloodGroup,
                                                          PrefixM=_apppersonal.PrefixM

                                                      }).ToList();

            List<MemberSearchResponse> memberSearchesFinalResult = (from s in memberSearches.Where(x => x.Qualification.ToLower().Contains("m.b.b.s") || x.Qualification.ToLower().Contains("b.a.m.s")
                                                                    || x.Qualification.ToLower().Contains("b.h.m.s")|| x.Qualification.ToLower().Contains("b.d.s")||x.Qualification.ToLower().Contains("m.d.s")
                                                                    ||x.Qualification.ToLower().Contains("md"))
                                                                    select new MemberSearchResponse()
                                                                    {
                                                                        MemberId = s.MemberId,
                                                                        FullName = $"{s.PrefixM} {s.FirstNameM} {s.MiddleNameM} {s.LastNameM}",
                                                                        Area = s.AreaM,
                                                                        City = s.CityM,
                                                                        MobileNumber = s.MobileNumber,
                                                                        Age = s.Age,
                                                                        BirthDate = s.BirthdateM,
                                                                        NativePlace = s.NativePlaceM,
                                                                        Qualification = s.QualificationM,
                                                                        BloodGroup=s.BloodGroup
                                                                    }).ToList();
            return memberSearchesFinalResult;
        }
        public List<MemberSearchResponse> GetAllMemberssSearchResultByFilterValues(Dictionary<string, string> keyValuePairs)
        {
            var pr = GetDynamicExpression(keyValuePairs);

            List<MemberSearchByDTO> memberSearches = (from _apppersonal in context.Member_PersonalDetails
                                                          join _appconatct in context.Member_ContactDetails
                                                          on _apppersonal.MemberId equals _appconatct.MemberId
                                                          join _appqualification in context.Member_EducationEmploymentDetails
                                                          on _apppersonal.MemberId equals _appqualification.MemberId
                                                          join _appFormStatus in context.Member_FormStatuses
                                                          on _apppersonal.MemberId equals _appFormStatus.MemberId
                                                          where _appFormStatus.FormStatus == "Approved"
                                                          select new MemberSearchByDTO
                                                          {
                                                              MemberId = _apppersonal.MemberId,
                                                              PrefixM=_apppersonal.PrefixM,
                                                              FirstName = _apppersonal.FirstName,
                                                              FirstNameM=_apppersonal.FirstNameM,
                                                              MiddleName = _apppersonal.MiddleName,
                                                              MiddleNameM=_apppersonal.MiddleNameM,
                                                              LastName = _apppersonal.LastName,
                                                              LastNameM=_apppersonal.LastNameM,
                                                              BirthDate = _apppersonal.BirthDate,
                                                              BirthdateM=_apppersonal.BirthDateM,
                                                              Qualification = _appqualification.Qualification,
                                                              QualificationM=_appqualification.QualificationM,
                                                              Proffession=_appqualification.Proffession,
                                                              MobileNumber = _appconatct.Mobile1,
                                                              NativePlace = _appconatct.NativePlace,
                                                              NativePlaceM=_appconatct.NativePlaceM,
                                                              NativePlaceTaluka=_appconatct.NativePlaceTaluka,
                                                              NativePlaceDistrict=_appconatct.NativePlaceDist,
                                                              Area = _apppersonal.Area,
                                                              AreaM=_apppersonal.AreaM,
                                                              City = _apppersonal.City,
                                                              CityM=_apppersonal.CityM,
                                                              Pincode=_apppersonal.Pincode,
                                                              District= _apppersonal.District!=null?_apppersonal.District:string.Empty,
                                                              Age=_apppersonal.Age,
                                                              BloodGroup=_apppersonal.BloodGroup,
                                                              Gender=_apppersonal.Gender

                                                          }).ToList();

            List<MemberSearchResponse> memberSearchesFinalResult = (from s in memberSearches.Where(pr)
                                                                    select new MemberSearchResponse()
                                                                    {
                                                                        MemberId = s.MemberId,
                                                                        FullName = $"{s.PrefixM} {s.FirstNameM} {s.MiddleNameM} {s.LastNameM}",
                                                                        Area = s.AreaM,
                                                                        City = s.CityM,
                                                                        MobileNumber = s.MobileNumber,
                                                                        Age = s.Age,
                                                                        BirthDate = s.BirthdateM,
                                                                        NativePlace = s.NativePlaceM,
                                                                        Qualification = s.QualificationM,
                                                                        BloodGroup=s.BloodGroup
                                                                    }).ToList();
            return memberSearchesFinalResult;
        }
        private ExpressionStarter<MemberSearchByDTO> GetDynamicExpression(Dictionary<string, string> keyValuePairs)
        {
            var pr = PredicateBuilder.New<MemberSearchByDTO>();
            foreach (var keyValuePair in keyValuePairs)
            {
                string key = keyValuePair.Key.ToString().ToLower();
                switch (key)
                {

                    case "firstname":
                        pr = pr.And(x => x.FirstName.ToLower().Contains(keyValuePair.Value.ToLower()) || x.FirstName.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "middlename":
                        pr = pr.And(x => x.MiddleName.ToLower().Contains(keyValuePair.Value.ToLower()) || x.MiddleName.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "lastname":
                        pr = pr.And(x => x.LastName.ToLower().Contains(keyValuePair.Value.ToLower()) || x.LastName.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "memberId":
                        pr = pr.And(x => x.MemberAppId.Contains(keyValuePair.Value.ToLower()));
                        break;
                    case "area":
                        pr = pr.And(x => x.Area.ToLower().Contains(keyValuePair.Value.ToLower()) || x.Area.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "city":
                        pr = pr.And(x => x.City.ToLower().Contains(keyValuePair.Value.ToLower()) || x.City.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "district":
                        pr = pr.And(x => x.District.ToLower().Contains(keyValuePair.Value.ToLower()) || x.District.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "pincode":
                        pr = pr.And(x => x.Pincode==Convert.ToInt32( keyValuePair.Value));
                        break;
                    case "nativeplace":
                        pr = pr.And(x => x.NativePlace.ToLower().Contains(keyValuePair.Value.ToLower()) || x.NativePlace.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "nativeplacetaluka":
                        pr = pr.And(x => x.NativePlaceTaluka.ToLower().Contains(keyValuePair.Value.ToLower()) || x.NativePlaceTaluka.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "nativeplacedistrict":
                        pr = pr.And(x => x.NativePlaceDistrict.ToLower().Contains(keyValuePair.Value.ToLower()) || x.NativePlaceDistrict.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "qualification":
                        pr = pr.And(x => x.Qualification.ToLower().Contains(keyValuePair.Value.ToLower()) || x.Qualification.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "proffession":
                        pr = pr.And(x => x.Proffession.ToLower().Contains(keyValuePair.Value.ToLower()) || x.Proffession.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "bloodgroup":
                        pr = pr.And(x => x.BloodGroup.ToLower().Contains(keyValuePair.Value.ToLower()) || x.BloodGroup.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "gender":
                        pr = pr.And(x => x.Gender.ToLower().Contains(keyValuePair.Value.ToLower()) || x.Gender.Contains(keyValuePair.Value.Trim()));
                        break;
                    default:
                        break;
                }

            }
            return pr;
        }
    }

    public interface IMemberSearchDataFactory
    {
        List<MemberSearchResponse> GetAllMemberssSearchResultByFilterValues(Dictionary<string, string> keyValuePairs);
        List<MemberSearchResponse> GetAllDoctors();
    }
}
