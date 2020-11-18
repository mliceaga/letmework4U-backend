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
    public class MeetingAdd
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingAdd(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        [FunctionName("MeetingAdd")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Meetings/MeetingAdd")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var meeting = JsonConvert.DeserializeObject<Core.Entities.Meeting>(await new StreamReader(req.Body).ReadToEndAsync());

                if (meeting.FollowUpMeetings.Any())
                {
                    foreach (var followUpMeeting in meeting.FollowUpMeetings)
                    {
                        followUpMeeting.ApplicantId = meeting.ApplicantId;
                        followUpMeeting.Id = Guid.NewGuid().ToString();
                        followUpMeeting.JobApplicationId = meeting.JobApplicationId;
                    }
                }

                if (meeting.Recruiters.Any())
                {
                    foreach (var recruiter in meeting.Recruiters)
                    {
                        recruiter.Id = Guid.NewGuid().ToString();
                    }
                }

                await _meetingRepository.AddItemAsync(meeting);
            }
            catch (Exception ex)
            {
                // TODO send 400 errors if it's the case, otherwise send 500.
                return new StatusCodeResult(500);
                // TO DO (add logger)
                throw ex;
            }
            return new OkResult();
        }
    }
}
