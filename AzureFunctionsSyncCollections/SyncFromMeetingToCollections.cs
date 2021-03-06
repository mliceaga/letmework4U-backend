using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Interfaces.Persistence;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSyncCollections
{
    public class SyncFromMeetingToCollections
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IApplicantTaskRepository _applicantTaskRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IRecruiterRepository _recruiterRepository;

        public SyncFromMeetingToCollections(IJobApplicationRepository jobApplicationRepository,
        IApplicantTaskRepository applicantTaskRepository,
        IMeetingRepository meetingRepository,
        IRecruiterRepository recruiterRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _applicantTaskRepository = applicantTaskRepository;
            _meetingRepository = meetingRepository;
            _recruiterRepository = recruiterRepository;
        }

        [FunctionName("SyncFromMeetingToCollections")]
        public void Run([CosmosDBTrigger(
            databaseName: "letmeworkCosmosDB",
            collectionName: "meetings",
            ConnectionStringSetting = "letmework4U_DOCUMENTDB",
            LeaseCollectionName = "leasesMeetings",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            try
            {
                if (input != null && input.Count > 0)
                {
                    var meeting = (Meeting)input;

                    foreach (var meetingFollowUp in meeting.FollowUpMeetings)
                    {
                        _meetingRepository.UpdateItemAsync(new Guid(meeting.ApplicantId), meetingFollowUp);
                    }

                    foreach (var recruiter in meeting.Recruiters)
                    {
                        _recruiterRepository.UpdateItemAsync(new Guid(recruiter.Id), recruiter);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO log exception
                throw ex;
            }
        }
    }
}
