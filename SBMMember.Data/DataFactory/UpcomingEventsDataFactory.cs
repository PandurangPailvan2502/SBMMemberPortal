using Microsoft.Extensions.Logging;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Data.DataFactory
{
   public class UpcomingEventsDataFactory: BaseMemberFactory<UpcomingEvent>, IUpcomingEventsDataFactory
    {
        private readonly SBMMemberDBContext SBMDBContext;
        private readonly ILogger<UpcomingEventsDataFactory> Logger;

        public UpcomingEventsDataFactory( SBMMemberDBContext dBContext, ILogger<UpcomingEventsDataFactory> _Logger)
        {
            SBMDBContext = dBContext;
            Logger = _Logger;
        }

        public override ResponseDTO AddDetails(UpcomingEvent upcoming)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                upcoming.Status = "Active";
                upcoming.CreateDate = DateTime.Now;

                int affectedRows = 0;
                SBMDBContext.UpcomingEvents.Add(upcoming);
                affectedRows = SBMDBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Upcoming Event details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding Upcoming Event details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Upcoming Event details save operation failed."
                };
            }

            return responseDTO;
        }

        public override UpcomingEvent GetDetailsByMemberId(int MemberId)
        {
            throw new NotImplementedException();
        }
        public UpcomingEvent GetDetailsById(int Id)
        {
            return SBMDBContext.UpcomingEvents.Where(x => x.Id == Id && x.Status == "Active").FirstOrDefault();
        }
        public void DeleteById(int Id)
        {
            UpcomingEvent events= SBMDBContext.UpcomingEvents.Where(x => x.Id == Id && x.Status == "Active").FirstOrDefault();
            events.Status = "InActive";
            SBMDBContext.SaveChanges();
        }
        public List<UpcomingEvent> GetAll()
        {
            return SBMDBContext.UpcomingEvents.Where(x=> x.Status == "Active").ToList();
        }
        public override ResponseDTO UpdateDetails(UpcomingEvent upcomingEvents)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                UpcomingEvent upcoming = SBMDBContext.UpcomingEvents.Where(x => x.Id ==upcomingEvents.Id).FirstOrDefault();
                upcoming.EventDate = upcomingEvents.EventDate;
                upcoming.EventTitle = upcomingEvents.EventTitle;
                upcoming.EventDesc = upcomingEvents.EventDesc;

                int affectedRows = 0;
                affectedRows = SBMDBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Upcoming Event details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating Upcoming Event details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Upcoming Event details update operation failed."
                };
            }

            return responseDTO;
        }
    }
    public interface IUpcomingEventsDataFactory
    {
        UpcomingEvent GetDetailsById(int Id);
        List<UpcomingEvent> GetAll();
        ResponseDTO UpdateDetails(UpcomingEvent upcomingEvents);
        ResponseDTO AddDetails(UpcomingEvent upcoming);
        void DeleteById(int Id);
    }
}
