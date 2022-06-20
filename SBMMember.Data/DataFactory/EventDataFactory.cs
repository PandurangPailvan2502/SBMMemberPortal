using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBMMember.Models;
using LinqKit;

namespace SBMMember.Data.DataFactory
{
   public class EventDataFactory: IEventDataFactory
    {
        private readonly SBMMemberDBContext eventDBContext;
        private readonly ILogger<EventDataFactory> Logger;
        public EventDataFactory(SBMMemberDBContext dBContext, ILogger<EventDataFactory> logger)
        {
            eventDBContext = dBContext;
            Logger = logger;
        }

        public EventInfo AddEventInfo(EventInfo _eventInfo)
        {
            var eventInfo = eventDBContext.EventInfos.Add(_eventInfo);
            var data = eventDBContext.SaveChanges();
            return eventInfo.Entity;
        }
        public void UpdateEventInfo(EventInfo eventInfo )
        {
            var eveInfo = eventDBContext.EventInfos.Where(x => x.EventId == eventInfo.EventId && x.Status=="Active").FirstOrDefault();
            eveInfo.EventName = eventInfo.EventName;
            eveInfo.EventDescription = eventInfo.EventDescription;
            eveInfo.EventYear = eventInfo.EventYear;
            eventDBContext.SaveChanges();
        }
        public void DeleteEventInfo(int eventId)
        {
            var eventInfo = eventDBContext.EventInfos.Where(x => x.EventId == eventId && x.Status == "Active").FirstOrDefault();
            eventInfo.Status = "InActive";
            eventDBContext.SaveChanges();
        }
        public ResponseDTO DeleteEventGalleryImageById(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();

            try
            {
                EventGallery gallery = eventDBContext.EventGalleries.Where(x => x.Id == id && x.Status == "Active").FirstOrDefault();
                gallery.Status = "InActive";

                int affectedrows = eventDBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Event Gallery Image deleted Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while deleting Event Gallery details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while deleting Event Gallery details by Id:{id}:{ex.Message}"
                };
            }
            return responseDTO;
        }
        public ResponseDTO AddEventGallery(EventGallery eventGallery)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var memberInfo = eventDBContext.EventGalleries.Add(eventGallery);
                int affectedrows = eventDBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Event Gallery Details added Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding Event Gallery details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while adding Event Gallery details:{ex.Message}"
                };
            }
            return responseDTO;
        }

        public List<EventInfo> GetEventListByEventParameters(Dictionary<string, string> keyValuePairs)
        {
            var pr = GetDynamicExpression(keyValuePairs);
            return eventDBContext.EventInfos.Where(pr).ToList();
        }
        public List<EventInfo> GetAllEventList()
        {

            return eventDBContext.EventInfos.Where(x => x.Status =="Active").ToList();
        }

        public EventInfo GetEventInfoByeventId(int eventId)
        {
            return eventDBContext.EventInfos.Where(x => x.EventId == eventId && x.Status=="Active").FirstOrDefault();
        }

        public List<EventGallery> GetGalleryphotosByeventId(int eventId)
        {
            return eventDBContext.EventGalleries.Where(x => x.EventId == eventId && x.Status == "Active").ToList();
        }


        private ExpressionStarter<EventInfo> GetDynamicExpression(Dictionary<string, string> keyValuePairs)
        {
            var pr = PredicateBuilder.New<EventInfo>();
            foreach (var keyValuePair in keyValuePairs)
            {
                string key = keyValuePair.Key.ToString().ToLower();
                switch (key)
                {

                    case "eventtitle":
                        pr = pr.And(x => x.EventName.ToLower().Contains(keyValuePair.Value.ToLower()) || x.EventName.Contains(keyValuePair.Value.Trim()));
                        break;
                    case "eventyear":
                        pr = pr.And(x => x.EventYear.ToLower().Contains(keyValuePair.Value.ToLower()) || x.EventYear.Contains(keyValuePair.Value.Trim()));
                        break;
                    
                    default:
                        break;
                }

            }
            return pr;
        }

    }

    public interface IEventDataFactory
    {
        EventInfo AddEventInfo(EventInfo _eventInfo);
        ResponseDTO AddEventGallery(EventGallery eventGallery);
        List<EventInfo> GetEventListByEventParameters(Dictionary<string, string> keyValuePairs);
        List<EventInfo> GetAllEventList();
        EventInfo GetEventInfoByeventId(int eventId);
        List<EventGallery> GetGalleryphotosByeventId(int eventId);
        void UpdateEventInfo(EventInfo eventInfo);
        void DeleteEventInfo(int eventId);
        ResponseDTO DeleteEventGalleryImageById(int id);
        
    }
}
