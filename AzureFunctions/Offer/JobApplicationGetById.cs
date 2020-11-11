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

namespace AzureFunctions.JobApplication
{
    public class JobApplicationGetById
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationGetById(
            IJobApplicationRepository jobJobApplicationRepository)
        {
            _jobApplicationRepository = jobJobApplicationRepository;
        }

        [FunctionName("JobApplicationGetById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "JobApplications/JobApplicationGetById/{id}")] HttpRequest req, string id, ILogger log)
        {
            var JobApplication = await _jobApplicationRepository.GetItemAsync(new Guid(id));

            if (JobApplication == null)
            {
                return new NotFoundObjectResult(id);
            }

            return new OkObjectResult(JobApplication);
        }
    }
}
