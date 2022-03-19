using Microsoft.Extensions.Logging;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Data.DataFactory
{
    public class EventAdsDataFactory: IEventAdsDataFactory
    {
        private readonly SBMMemberDBContext eventDBContext;
        private readonly ILogger<EventAdsDataFactory> Logger;

        public EventAdsDataFactory(SBMMemberDBContext dBContext, ILogger<EventAdsDataFactory> logger)
        {
            eventDBContext = dBContext;
            Logger = logger;
        }
        public ResponseDTO AddEventAds(EventAds eventAds)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var memberInfo = eventDBContext.EventAds.Add(eventAds);
                int affectedrows = eventDBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Event Ads Details added Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding Event Ads details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while adding Event Ads details:{ex.Message}"
                };
            }
            return responseDTO;
        }

        public List<EventAds> GetEventAdsByYear(string year)
        {
            return eventDBContext.EventAds.Where(x => x.EventYear == year).ToList();
        }
        public List<EventAds> GetAllEventAds()
        {
            return eventDBContext.EventAds.Where(x => x.Status == "Active").ToList();
        }
    }

    public interface IEventAdsDataFactory
    {
        ResponseDTO AddEventAds(EventAds eventAds);
        List<EventAds> GetEventAdsByYear(string year);
        List<EventAds> GetAllEventAds();
    }
}
