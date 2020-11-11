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
    public class OfferGetById
    {
        private readonly IOfferRepository _offerRepository;

        public OfferGetById(
            IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        [FunctionName("OfferGetById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "offers/offerGetById/{id}")] HttpRequest req, string id, ILogger log)
        {
            var offer = await _offerRepository.GetItemAsync(new Guid(id));

            if (offer == null)
            {
                return new NotFoundObjectResult(id);
            }

            return new OkObjectResult(offer);
        }
    }
}
