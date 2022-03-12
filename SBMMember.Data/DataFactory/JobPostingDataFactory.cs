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
    }
    public interface IJobPostingDataFactory
    {
        List<JobPostings> GetJobPostings();
    }
}
