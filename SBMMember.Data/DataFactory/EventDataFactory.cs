using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBMMember.Models;
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

        public List<EventInfo> GetEventListByEventYear(string year)
        {

            return eventDBContext.EventInfos.Where(x => x.EventYear == year).ToList();
        }
        public List<EventInfo> GetAllEventList()
        {

            return eventDBContext.EventInfos.Where(x => x.Status =="Active").ToList();
        }

    }

    public interface IEventDataFactory
    {
        EventInfo AddEventInfo(EventInfo _eventInfo);
        ResponseDTO AddEventGallery(EventGallery eventGallery);
        List<EventInfo> GetEventListByEventYear(string year);
        List<EventInfo> GetAllEventList();
    }
}
