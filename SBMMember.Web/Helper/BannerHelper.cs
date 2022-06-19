using Newtonsoft.Json;
using RestSharp;
using SBMMember.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Data.DataFactory;
namespace SBMMember.Web.Helper
{
    public class BannerHelper
    {
        
        public static List<BannerClass> GetBanners()
        {
            Dictionary<string, string> banners = new Dictionary<string, string>();

            RestClient restClient = new RestClient("https://samatabhratrumandal.com/RestAPI/api/Banners/GetAllBanners");

            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");

            var response = restClient.Get(request);
            List<BannerClass> _banner = JsonConvert.DeserializeObject<List<BannerClass>>(response.Content.ToString());
            //foreach (BannerClass item in _banner)
            //{
            //    banners.Add(item.heading, $"https://samatabhratrumandal.com/RestAPI/{item.imageURL}");
            //}
            return _banner.Select(x => new BannerClass()
            {
                id = x.id,
                heading = x.heading,
                imageURL = "https://samatabhratrumandal.com/RestAPI" + x.imageURL

            }).ToList();
        }

        public static ResponseDTO UploadBanner(byte[] file, string heading, string fileName)
        {
            ResponseDTO retResponse = new ResponseDTO();

            RestClient restClient = new RestClient($"https://samatabhratrumandal.com/RestAPI/api/Banners/uploadBanner/upload?heading={heading}");

            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");
            request.AddFile("file", file, fileName);

            IRestResponse response = restClient.Post(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                retResponse.Result = "Success";
                retResponse.Message = "Banner image uploaded successfully.";
            }
            else
            {
                retResponse.Result = "Failed";
                retResponse.Message = "Banner image upload operation failed.";
            }

            return retResponse;
        }

        public static ResponseDTO RemoveBanner(int id)
        {
            ResponseDTO retResponse = new ResponseDTO();

            RestClient restClient = new RestClient($"https://samatabhratrumandal.com/RestAPI/api/Banners/RemoveBanner/{id}");

            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");
            //request.AddFile("file", file, fileName);

            IRestResponse response = restClient.Post(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                retResponse.Result = "Success";
                retResponse.Message = "Banner image Remove successfully.";
            }
            else
            {
                retResponse.Result = "Failed";
                retResponse.Message = "Banner image remove operation failed.";
            }

            return retResponse;
        }
    }
}
