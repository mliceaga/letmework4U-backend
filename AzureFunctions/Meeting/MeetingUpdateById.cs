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

namespace AzureFunctions.Meeting
{
    public class MeetingUpdateById
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingUpdateById(
            IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        [FunctionName("MeetingUpdateById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "Meetings/{id}")] HttpRequest req, string id, ILogger log)
        {
            var meetings = JsonConvert.DeserializeObject<Core.Entities.Meeting>(await new StreamReader(req.Body).ReadToEndAsync());

            await _meetingRepository.UpdateItemAsync(new Guid(id), meetings);

            return new OkObjectResult(meetings);
        }
    }
}
