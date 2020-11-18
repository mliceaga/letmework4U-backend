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
    public class MeetingGetById
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingGetById(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        [FunctionName("MeetingGetById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Meetings/MeetingGetById/{id}")] HttpRequest req, string id, ILogger log)
        {
            var meeting = await _meetingRepository.GetItemAsync(new Guid(id));

            if (meeting == null)
            {
                return new NotFoundObjectResult(id);
            }

            return new OkObjectResult(meeting);
        }
    }
}
