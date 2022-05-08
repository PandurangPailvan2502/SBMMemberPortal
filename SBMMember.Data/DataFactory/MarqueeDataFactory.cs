using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
   public class MarqueeDataFactory: IMarqueeDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger<MarqueeDataFactory> Logger;
        public MarqueeDataFactory(SBMMemberDBContext context, ILogger<MarqueeDataFactory> logger)
        {
            dBContext = context;
            Logger = logger;
        }
        public ResponseDTO AddMarqueeDetails(MarqueeText marqueeText)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                marqueeText.Createdate = DateTime.Now;
                marqueeText.Status = "Active";

                var memberInfo = dBContext.MarqueeTexts.Add(marqueeText);
                int affectedrows = dBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Marquee Details added Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding Marquee details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while adding Marquee details:{ex.Message}"
                };
            }
            return responseDTO;
        }
        public ResponseDTO UpdateMarqueetails(MarqueeText marqueeText)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var _marqueeText = dBContext.MarqueeTexts.Where(x => x.Id == marqueeText.Id && x.Status == "Active").FirstOrDefault();
                _marqueeText.Marquee = marqueeText.Marquee;
               

                int affectedrows = dBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Marquee Details updated Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating Marquee details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while updating Marquee details:{ex.Message}"
                };
            }
            return responseDTO;
        }
        public ResponseDTO DeleteMarqueeDetails(int Id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var jobPost = dBContext.JobPostings.Where(x => x.Id == Id && x.Status == "Active").FirstOrDefault();

                jobPost.Status = "InActive";

                int affectedrows = dBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Marquee Details Deleted Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while deleting Marquee details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while deleting Marquee details:{ex.Message}"
                };
            }
            return responseDTO;
        }
        public List<MarqueeText> GetMarquees()
        {
            return dBContext.MarqueeTexts.Where(x=>x.Status=="Active").ToList();
        }

        public MarqueeText GetMarqueeById(int Id)
        {
            return dBContext.MarqueeTexts.Where(x => x.Id==Id&& x.Status == "Active").FirstOrDefault();
        }
    }

    public interface IMarqueeDataFactory
    {
        List<MarqueeText> GetMarquees();
        ResponseDTO AddMarqueeDetails(MarqueeText marqueeText);
        ResponseDTO UpdateMarqueetails(MarqueeText marqueeText);
        ResponseDTO DeleteMarqueeDetails(int Id);
        MarqueeText GetMarqueeById(int Id);
    }
}
