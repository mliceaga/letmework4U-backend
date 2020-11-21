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
    public class ApplicantGetById
    {
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantGetById(
            IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        [FunctionName("ApplicantGetById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Applicants/ApplicationGetById/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            try
            {
                var applicant = await _applicantRepository.GetItemAsync(new Guid(id));

                if (applicant == null)
                {
                    return new NotFoundObjectResult(id);
                }
                return new OkObjectResult(applicant);
            }
            catch (Exception ex)
            {
                // TODO Handle exception
                return new StatusCodeResult(500); ;
            }
        }
    }
}
