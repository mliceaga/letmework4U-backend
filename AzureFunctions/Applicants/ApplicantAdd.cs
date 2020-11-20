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
using System.Linq;

namespace AzureFunctions.Applicant
{
    public class ApplicantAdd
    {
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantAdd(
            IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        [FunctionName("ApplicantAdd")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Applicants/ApplicantAdd")] HttpRequest req,
            ILogger log)
        {
            Core.Entities.Applicant applicantInserted = null;
            try
            {
                var applicant = JsonConvert.DeserializeObject<Core.Entities.Applicant>(await new StreamReader(req.Body).ReadToEndAsync());

                applicant.CreatedOnDate = DateTime.UtcNow;

                var applicantId = await _applicantRepository.AddItemAsync(applicant);

                applicantInserted = await _applicantRepository.GetItemAsync(new Guid(applicantId));
            }
            catch (Exception ex)
            {
                // TODO send 400 errors if it's the case, otherwise send 500.
                return new StatusCodeResult(500);
                // TO DO (add logger)
                throw ex;
            }
            return new OkObjectResult(applicantInserted);
        }
    }
}
