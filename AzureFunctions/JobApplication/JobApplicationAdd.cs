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
using System.Linq;

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
            try
            {
                var jobApplication = JsonConvert.DeserializeObject<Core.Entities.JobApplication>(await new StreamReader(req.Body).ReadToEndAsync());

                if (jobApplication.Company != null)
                {
                    jobApplication.Company.Id = Guid.NewGuid().ToString();
                }

                if (jobApplication.ApplicantTasks.Any())
                {
                    foreach (var applicantTask in jobApplication.ApplicantTasks)
                    {
                        applicantTask.ApplicantId = jobApplication.ApplicantId;
                        applicantTask.Id = Guid.NewGuid().ToString();
                    }
                }

                if (jobApplication.Recruiters.Any())
                {
                    foreach (var applicantTask in jobApplication.Recruiters)
                    {
                        applicantTask.Id = Guid.NewGuid().ToString();
                    }
                }

                if (jobApplication.FirstMeeting != null)
                {
                    jobApplication.FirstMeeting.ApplicantId = jobApplication.ApplicantId;
                    jobApplication.FirstMeeting.Id = Guid.NewGuid().ToString();
                }

                await _jobApplicationRepository.AddItemAsync(jobApplication);
            }
            catch (Exception ex)
            {
                // TODO send 400 errors if it's the case, otherwise send 500.
                return new StatusCodeResult(500);
                // TO DO (add logger)
                throw ex;
            }
            return new OkResult();
        }
    }
}
