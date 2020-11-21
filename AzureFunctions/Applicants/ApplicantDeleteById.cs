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
    public class ApplicantDeleteById
    {
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantDeleteById(
            IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        [FunctionName("ApplicantDeleteById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Applicants/ApplicantDeleteById/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            try
            { 
                await _applicantRepository.DeleteItemAsync(new Guid(id));
            }
            catch(Exception ex)
            {
                // TODO Handle exception
                throw ex;
            }
            return new NoContentResult();
        }
    }
}
