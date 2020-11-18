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
    public class SyncFromJobApplicationToCollections
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IApplicantTaskRepository _applicantTaskRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly ICompanyRepository _companyRepository;

        public SyncFromJobApplicationToCollections(IJobApplicationRepository jobApplicationRepository,
        IApplicantTaskRepository applicantTaskRepository,
        IMeetingRepository meetingRepository,
        IRecruiterRepository recruiterRepository,
        ICompanyRepository companyRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _applicantTaskRepository = applicantTaskRepository;
            _meetingRepository = meetingRepository;
            _recruiterRepository = recruiterRepository;
            _companyRepository = companyRepository;
        }

        [FunctionName("SyncFromJobApplicationToCollections")]
        public void Run([CosmosDBTrigger(
            databaseName: "letmeworkCosmosDB",
            collectionName: "jobApplications",
            ConnectionStringSetting = "letmework4U_DOCUMENTDB",
            LeaseCollectionName = "leasesJobApplications",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("jobApplications modified " + input.Count);
                log.LogInformation("First jobApplication Id " + input[0].Id);
                var jobApplication = (JobApplication)input;
                
                _companyRepository.AddOrUpdateAsync(jobApplication.Company, new Microsoft.Azure.Cosmos.PartitionKey(jobApplication.Company.Name));

                _meetingRepository.AddOrUpdateAsync(jobApplication.FirstMeeting, new Microsoft.Azure.Cosmos.PartitionKey(jobApplication.ApplicantId));

                var applicantIdPartitionKey = new Microsoft.Azure.Cosmos.PartitionKey(jobApplication.ApplicantId);

                foreach (var appTask in jobApplication.ApplicantTasks)
                {
                    _applicantTaskRepository.AddOrUpdateAsync(appTask, applicantIdPartitionKey);
                }

                foreach(var recruiter in jobApplication.Recruiters)
                {
                    _recruiterRepository.AddOrUpdateAsync(recruiter, new Microsoft.Azure.Cosmos.PartitionKey(recruiter.Lastname));
                }

            }
        }
    }
}
