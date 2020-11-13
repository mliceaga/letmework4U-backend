using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Core.Interfaces.Persistence;
using Microsoft.Azure.Cosmos;

namespace AzureFunctions.JobApplication
{
    public class JobApplicationList
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationList(
            IJobApplicationRepository jobJobApplicationRepository)
        {
            _jobApplicationRepository = jobJobApplicationRepository;
        }

        [FunctionName("JobApplicationList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "JobApplications")] HttpRequest req,
            ILogger log)
        {
            var JobApplicationsList = await _jobApplicationRepository.GetItemsAsync("select * from c");

            return new OkObjectResult(JobApplicationsList);
        }
    }
}
