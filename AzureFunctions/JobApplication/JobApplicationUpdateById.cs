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
            try
            {
                var jobApplicationItem = JsonConvert.DeserializeObject<Core.Entities.JobApplication>(await new StreamReader(req.Body).ReadToEndAsync());
                var applicantIdPartitionKey = new Microsoft.Azure.Cosmos.PartitionKey(jobApplicationItem.ApplicantId);

                var savedJobApplicationItem = await _jobApplicationRepository.AddOrUpdateAsync(jobApplicationItem, applicantIdPartitionKey);

                return new OkObjectResult(savedJobApplicationItem);
            }
            catch (Exception ex)
            {
                // TODO send 400 errors if it's the case, otherwise send 500.
                return new StatusCodeResult(500);
                // TO DO (add logger)
                throw ex;
            }
        }
    }
}
