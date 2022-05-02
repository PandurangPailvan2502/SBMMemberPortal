using Microsoft.Extensions.Logging;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Data.DataFactory
{
    public class EventTitlesDataFactory : IEventTitlesDataFactory
    {
        SBMMemberDBContext dBContext;
        ILogger<EventTitlesDataFactory> Logger;
        public EventTitlesDataFactory(SBMMemberDBContext _dBContext, ILogger<EventTitlesDataFactory> _logger)
        {
            dBContext = _dBContext;
            Logger = _logger;
        }
        public ResponseDTO AddDetails(EventTitles eventTitle)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            eventTitle.Status = "Active";
           
            try
            {
                int affectedRows = 0;
                dBContext.EventTitles.Add(eventTitle);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Event Title details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding event Title details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Event Title details save operation failed."
                };
            }

            return responseDTO;
        }

        public ResponseDTO UpdateDetails(EventTitles eventTitle)
        {
            ResponseDTO responseDTO = new ResponseDTO();


            try
            {
                EventTitles titles = dBContext.EventTitles.Where(x => x.Id == eventTitle.Id && x.Status == "Active").FirstOrDefault();
                titles.EventTitle = eventTitle.EventTitle;
                titles.EventTitleM = eventTitle.EventTitleM;
                int affectedRows = 0;
               
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Event Title details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating event Title details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Event Title details update operation failed."
                };
            }

            return responseDTO;
        }

        public ResponseDTO DeleteEventTitle(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();


            try
            {
                EventTitles titles = dBContext.EventTitles.Where(x => x.Id == id && x.Status == "Active").FirstOrDefault();
                titles.Status = "InActive";
                int affectedRows = 0;

                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Event Title details deleted successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while deleting event Title details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Event Title details delete operation failed."
                };
            }

            return responseDTO;
        }
        public List<EventTitles> GetEventTitles()
        {
            return dBContext.EventTitles.Where(x => x.Status == "Active").ToList();
        }

        public EventTitles GetEventTitleGetById(int id)
        {
            return dBContext.EventTitles.Where(x => x.Status == "Active" && x.Id==id).FirstOrDefault();
        }

    }
    public interface IEventTitlesDataFactory
    {
        ResponseDTO AddDetails(EventTitles eventTitle);
        ResponseDTO UpdateDetails(EventTitles eventTitle);
        List<EventTitles> GetEventTitles();
        ResponseDTO DeleteEventTitle(int id);
        EventTitles GetEventTitleGetById(int id);
    }

}
