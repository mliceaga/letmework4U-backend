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
    public class JobApplicationUpdateById
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationUpdateById(
            IJobApplicationRepository jobJobApplicationRepository)
        {
            _jobApplicationRepository = jobJobApplicationRepository;
        }

        [FunctionName("JobApplicationUpdateById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "JobApplications/{id}")] HttpRequest req, string id, ILogger log)
        {
            var JobApplicationOptions = JsonConvert.DeserializeObject<Core.Entities.JobApplication>(await new StreamReader(req.Body).ReadToEndAsync());

            await _jobApplicationRepository.UpdateItemAsync(new Guid(id), JobApplicationOptions);

            return new OkObjectResult(JobApplicationOptions);
        }
    }
}
