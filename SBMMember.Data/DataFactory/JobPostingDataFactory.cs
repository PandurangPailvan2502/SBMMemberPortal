using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class JobPostingDataFactory: IJobPostingDataFactory
    {
        private readonly SBMMemberDBContext memberDBContext;
        private readonly ILogger<JobPostingDataFactory> Logger;
        public JobPostingDataFactory( SBMMemberDBContext dBContext, ILogger<JobPostingDataFactory> logger)
        {
            memberDBContext = dBContext;
            Logger = logger;
        }

        public List<JobPostings> GetJobPostings()
        {
            return memberDBContext.JobPostings.ToList();
        }

        public ResponseDTO AddJobDetails(JobPostings jobPostings)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                jobPostings.PostedOn = DateTime.Now;
                var memberInfo = memberDBContext.JobPostings.Add(jobPostings);
                int affectedrows = memberDBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Job Details added Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding Job details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while adding Job details:{ex.Message}"
                };
            }
            return responseDTO;
        }
    }
    public interface IJobPostingDataFactory
    {
        List<JobPostings> GetJobPostings();
        ResponseDTO AddJobDetails(JobPostings jobPostings);

    }
}
