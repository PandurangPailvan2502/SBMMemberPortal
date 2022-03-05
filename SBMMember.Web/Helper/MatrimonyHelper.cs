using Newtonsoft.Json;
using RestSharp;
using SBMMember.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Helper
{
    public class MatrimonyHelper
    {
        public static List<CandidateSearchResponse> GetAllVadhus()
        {
            Dictionary<string, string> banners = new Dictionary<string, string>();

            RestClient restClient = new RestClient("https://samatabhratrumandal.com/RestAPI/api/candidate/GetAllCandidatesSearchResult/21");

            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");

            var response = restClient.Get(request);
            List<CandidateSearchResponse> _banner = JsonConvert.DeserializeObject<List<CandidateSearchResponse>>(response.Content.ToString());
            
            return _banner.Select(x => new CandidateSearchResponse()
            {
                candidateId=x.candidateId,
                fullName=x.fullName,
                age=x.age,
                education=x.education,
                profileImage = "https://samatabhratrumandal.com" + x.profileImage

            }).ToList();
        }

        public static List<CandidateSearchResponse> GetAllVar()
        {
            Dictionary<string, string> banners = new Dictionary<string, string>();

            RestClient restClient = new RestClient("https://samatabhratrumandal.com/RestAPI/api/candidate/GetAllCandidatesSearchResult/27");

            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");

            var response = restClient.Get(request);
            List<CandidateSearchResponse> _banner = JsonConvert.DeserializeObject<List<CandidateSearchResponse>>(response.Content.ToString());

            return _banner.Select(x => new CandidateSearchResponse()
            {
                candidateId = x.candidateId,
                fullName = x.fullName,
                age = x.age,
                education = x.education,
                profileImage = "https://samatabhratrumandal.com" + x.profileImage

            }).ToList();
        }
    }
}
