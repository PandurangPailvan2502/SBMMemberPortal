using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<BannerClass> _banner= JsonConvert.DeserializeObject<List< BannerClass>>(response.Content.ToString());
            //foreach (BannerClass item in _banner)
            //{
            //    banners.Add(item.heading, $"https://samatabhratrumandal.com/RestAPI/{item.imageURL}");
            //}
            return _banner.Select(x=>new BannerClass() { 
               id=x.id,
               heading=x.heading,
               imageURL= "https://samatabhratrumandal.com/RestAPI"+x.imageURL

            }).ToList();
        }

        
    }
}
