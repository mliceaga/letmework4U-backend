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
using Microsoft.Azure.Cosmos;

namespace AzureFunctions.Offer
{
    public class OfferList
    {
        private readonly IOfferRepository _offerRepository;

        public OfferList(
            IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        [FunctionName("OfferList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "offers")] HttpRequest req,
            ILogger log)
        {
            var offersList = await _offerRepository.GetItemsAsync("select * from c");

            return new OkObjectResult(offersList);
        }
    }
}
