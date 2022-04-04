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
        public ResponseDTO UpdateJobDetails(JobPostings jobPostings)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var jobPost = memberDBContext.JobPostings.Where(x => x.Id == jobPostings.Id && x.Status == "Active").FirstOrDefault();
                jobPost.JobTitle = jobPostings.JobTitle;
                jobPost.JobDescription = jobPostings.JobDescription;
                jobPost.JobLocation = jobPostings.JobLocation;
                jobPost.PositionFor = jobPostings.PositionFor;
                jobPost.PostedOn = jobPostings.PostedOn;
                jobPost.Qualification = jobPostings.Qualification;
                jobPost.SalaryBand = jobPostings.SalaryBand;
                jobPost.Status = jobPostings.Status;
              
                int affectedrows = memberDBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Job Details updated Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating Job details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while updating Job details:{ex.Message}"
                };
            }
            return responseDTO;
        }
        public ResponseDTO DeleteJobDetails(JobPostings jobPostings)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var jobPost = memberDBContext.JobPostings.Where(x => x.Id == jobPostings.Id && x.Status == "Active").FirstOrDefault();
               
                jobPost.Status ="InActive";

                int affectedrows = memberDBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Job Details Deleted Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while deleting Job details:{ex.Message}");
                return responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Error occured while deleting Job details:{ex.Message}"
                };
            }
            return responseDTO;
        }
    }
    public interface IJobPostingDataFactory
    {
        List<JobPostings> GetJobPostings();
        ResponseDTO AddJobDetails(JobPostings jobPostings);
        ResponseDTO UpdateJobDetails(JobPostings jobPostings);
        ResponseDTO DeleteJobDetails(JobPostings jobPostings);


    }
}
