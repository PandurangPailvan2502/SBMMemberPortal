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
        public List<MemberSearchResponse> GetAllMemberssSearchResultByFilterValues(Dictionary<string, string> keyValuePairs)
        {
            var pr = GetDynamicExpression(keyValuePairs);

            List<MemberSearchByNameDTO> memberSearches = (from _apppersonal in context.Member_PersonalDetails
                                                          join _appconatct in context.Member_ContactDetails
                                                          on _apppersonal.MemberId equals _appconatct.MemberId
                                                          join _appqualification in context.Member_EducationEmploymentDetails
                                                          on _apppersonal.MemberId equals _appqualification.MemberId
                                                          join _appFormStatus in context.Member_FormStatuses
                                                          on _apppersonal.MemberId equals _appFormStatus.MemberId
                                                          where _appFormStatus.FormStatus == "Approved"
                                                          select new MemberSearchByNameDTO
                                                          {
                                                              MemberId = _apppersonal.MemberId,
                                                              FirstName = _apppersonal.FirstName,
                                                              MiddleName = _apppersonal.MiddleName,
                                                              LastName = _apppersonal.LastName,
                                                              BirthDate = _apppersonal.BirthDate,
                                                              Qualification = _appqualification.Qualification,
                                                              MobileNumber = _appconatct.Mobile1,
                                                              NativePlace = _appconatct.NativePlace,
                                                              Area = _apppersonal.Area,
                                                              City = _apppersonal.City,
                                                              Pincode=_apppersonal.Pincode,
                                                              District= _apppersonal.District!=null?_apppersonal.District:string.Empty,
                                                              Age=_apppersonal.Age

                                                          }).ToList();

            List<MemberSearchResponse> memberSearchesFinalResult = (from s in memberSearches.Where(pr)
                                                                    select new MemberSearchResponse()
                                                                    {
                                                                        MemberId = s.MemberId,
                                                                        FullName = $"{s.FirstName} {s.MiddleName} {s.LastName}",
                                                                        Area = s.Area,
                                                                        City = s.City,
                                                                        MobileNumber = s.MobileNumber,
                                                                        Age = s.Age,
                                                                        BirthDate = s.BirthDate,
                                                                        NativePlace = s.NativePlace,
                                                                        Qualification = s.Qualification
                                                                    }).ToList();
            return memberSearchesFinalResult;
        }
        private ExpressionStarter<MemberSearchByNameDTO> GetDynamicExpression(Dictionary<string, string> keyValuePairs)
        {
            var pr = PredicateBuilder.New<MemberSearchByNameDTO>();
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
    }
}
