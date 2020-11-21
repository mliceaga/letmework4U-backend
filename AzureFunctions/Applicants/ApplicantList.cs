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
    public class ApplicantList
    {
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantList(
            IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        [FunctionName("ApplicantList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Applicants")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var applicantsList = await _applicantRepository.GetItemsAsync("select * from c");
                return new OkObjectResult(applicantsList);
            }
            catch(Exception ex)
            {
                // TODO send 400 errors if it's the case, otherwise send 500.
                return new StatusCodeResult(500);
                // TO DO (add logger)
                throw ex;
            }
            
        }
    }
}
