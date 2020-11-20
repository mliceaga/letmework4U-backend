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
    public class JobApplicationDeleteById
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationDeleteById(
            IJobApplicationRepository jobJobApplicationRepository)
        {
            _jobApplicationRepository = jobJobApplicationRepository;
        }

        [FunctionName("JobApplicationDeleteById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "JobApplications/JobApplicationDeleteById/{id}")] HttpRequest req, string id, ILogger log)
        {
            await _jobApplicationRepository.DeleteItemAsync(new Guid(id));

            return new NoContentResult();
        }
    }
}
