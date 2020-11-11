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

namespace AzureFunctions.Offer
{
    public class OfferUpdateById
    {
        private readonly IOfferRepository _offerRepository;

        public OfferUpdateById(
            IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        [FunctionName("OfferUpdateById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "offers/{id}")] HttpRequest req, string id, ILogger log)
        {
            var offerOptions = JsonConvert.DeserializeObject<Core.Entities.Offer>(await new StreamReader(req.Body).ReadToEndAsync());

            // TODO verify that the item returned comes from the UpdateItemAsync and not the same offerOptions
            await _offerRepository.UpdateItemAsync(new Guid(id), offerOptions);

            return new OkObjectResult(offerOptions);
        }
    }
}
