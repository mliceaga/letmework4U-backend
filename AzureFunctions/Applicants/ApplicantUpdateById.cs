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

namespace AzureFunctions.Applicant
{
    public class ApplicantUpdateById
    {
        private readonly IApplicantRepository _applicantRepository;
        public ApplicantUpdateById(
            IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        [FunctionName("ApplicantUpdateById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "Applicants/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            try
            {
                var applicantItem = JsonConvert.DeserializeObject<Core.Entities.Applicant>(await new StreamReader(req.Body).ReadToEndAsync());
                var applicantIdPartitionKey = new Microsoft.Azure.Cosmos.PartitionKey(applicantItem.Id);

                await _applicantRepository.UpdateItemAsync(new Guid(id), applicantItem);

                return new OkObjectResult(applicantItem);
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
