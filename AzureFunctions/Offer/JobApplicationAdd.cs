using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Core.Entities;
using Infrastructure.CosmosDbData.Repository;
using Core.Interfaces.Persistence;

namespace AzureFunctions.JobApplication
{
    public class JobApplicationAdd
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationAdd(
            IJobApplicationRepository jobJobApplicationRepository)
        {
            _jobApplicationRepository = jobJobApplicationRepository;
        }

        [FunctionName("JobApplicationAdd")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "JobApplications/JobApplicationAdd")] HttpRequest req,
            ILogger log)
        {
            var JobApplication = JsonConvert.DeserializeObject<Core.Entities.JobApplication>(await new StreamReader(req.Body).ReadToEndAsync());

            await _jobApplicationRepository.AddItemAsync(JobApplication);

            return new OkResult();
        }
    }
}
