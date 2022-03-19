using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Data.DataFactory;
using SBMMember.Models;
using SBMMember.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace SBMMember.Web.Controllers
{
    [Authorize]
    public class JobDirectoryController : Controller
    {
        private readonly ILogger<JobDirectoryController> Logger;
        private readonly IMapper mapper;
        private readonly IJobPostingDataFactory jobPostingDataFactory;
        public JobDirectoryController(ILogger<JobDirectoryController> logger,IMapper _mapper,IJobPostingDataFactory dataFactory)
        {
            Logger = logger;
            mapper = _mapper;
            jobPostingDataFactory = dataFactory;
        }
        public IActionResult JobDirectory()
        {
            List<JobPostings> jobPostings = jobPostingDataFactory.GetJobPostings();
            List<JobPostingViewModel> jobs = new List<JobPostingViewModel>();
            foreach (JobPostings job in jobPostings)
            {
                JobPostingViewModel model = mapper.Map<JobPostingViewModel>(job);
                jobs.Add(model);
            }
            JobPostingViewModel postingViewModel = new JobPostingViewModel() { 
            JobPostings=jobs
            };
            return View(postingViewModel);
        }
    }
}
