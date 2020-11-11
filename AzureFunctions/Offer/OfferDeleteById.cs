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
    public class OfferDeleteById
    {
        private readonly IOfferRepository _offerRepository;

        public OfferDeleteById(
            IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        [FunctionName("OfferDeleteById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "offers/offerDeleteById/{id}")] HttpRequest req, string id, ILogger log)
        {
            await _offerRepository.DeleteItemAsync(new Guid(id));

            return new NoContentResult();
        }
    }
}
