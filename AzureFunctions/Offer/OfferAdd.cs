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

namespace AzureFunctions.Offer
{
    public class OfferAdd
    {
        private readonly IOfferRepository _offerRepository;

        public OfferAdd(
            IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        [FunctionName("OfferAdd")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "offers/offerAdd")] HttpRequest req,
            ILogger log)
        {
            var offer = JsonConvert.DeserializeObject<Core.Entities.Offer>(await new StreamReader(req.Body).ReadToEndAsync());

            await _offerRepository.AddItemAsync(offer);

            return new OkResult();
        }
    }
}
