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
using System.Linq;

namespace AzureFunctions.Meeting
{
    public class MeetingList
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingList(
            IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        [FunctionName("MeetingList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Meetings")] HttpRequest req,
            ILogger log)
        {
            var meetings = await _meetingRepository.GetItemsAsync("select * from c");

            return new OkObjectResult(meetings);
        }
    }
}
