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
    public class MeetingDeleteById
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingDeleteById(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        [FunctionName("MeetingDeleteById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Meetings/MeetingDeleteById/{id}")] HttpRequest req, string id, ILogger log)
        {
            await _meetingRepository.DeleteItemAsync(new Guid(id));

            return new NoContentResult();
        }
    }
}
