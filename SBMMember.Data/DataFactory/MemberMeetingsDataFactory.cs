using Microsoft.Extensions.Logging;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Data.DataFactory
{
    public class MemberMeetingsDataFactory: IMemberMeetingsDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger<MemberMeetingsDataFactory> logger;
        public MemberMeetingsDataFactory(SBMMemberDBContext memberDBContext, ILogger<MemberMeetingsDataFactory> _logger)
        {
            dBContext = memberDBContext;
            logger = _logger;
        }
        public ResponseDTO AddMeetingDetails(MemberMeetings memberMeetings)
        {

            memberMeetings.CreateDate = DateTime.Now;
            memberMeetings.IsActive = 1;
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                dBContext.MemberMeetings.Add(memberMeetings);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member meeting details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while adding member meeting details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Member meeting details save operation failed."
                };
            }

            return responseDTO;
        }

        public ResponseDTO UpdateMeetingDetails(MemberMeetings memberMeetings)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                MemberMeetings meetings = dBContext.MemberMeetings.Where(x => x.Id == memberMeetings.Id && x.IsActive==1).First();
                meetings.MeetingURL = memberMeetings.MeetingURL;
                meetings.Status = memberMeetings.Status;
                meetings.MeetingDate = memberMeetings.MeetingDate;


                int affectedRows = 0;
                //dBContext.Update(personalDetails);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Meeting details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while updating Meeting details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Meeting details update operation failed."
                };
            }

            return responseDTO;
        }
        public ResponseDTO DeleteMeetingDetails(int Id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                MemberMeetings meetings = dBContext.MemberMeetings.Where(x => x.Id == Id && x.IsActive==1).First();
                meetings.IsActive = 0;


                int affectedRows = 0;
                //dBContext.Update(personalDetails);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Meeting details deleted successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while deleting Meeting details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Meeting details delete operation failed."
                };
            }

            return responseDTO;
        }
        public List<MemberMeetings> GetMemberMeetings()
        {
            return dBContext.MemberMeetings.Where(x => x.IsActive == 1).ToList();
        }
        public MemberMeetings GetMemberMeetingById(int Id)
        {
            return dBContext.MemberMeetings.Where(x => x.IsActive == 1 && x.Id==Id).FirstOrDefault();
        }

    }
    public interface IMemberMeetingsDataFactory
    {
        ResponseDTO AddMeetingDetails(MemberMeetings memberMeetings);
        MemberMeetings GetMemberMeetingById(int Id);
        List<MemberMeetings> GetMemberMeetings();
        ResponseDTO DeleteMeetingDetails(int Id);
        ResponseDTO UpdateMeetingDetails(MemberMeetings memberMeetings);
    }
}
