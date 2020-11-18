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
        private readonly IJobApplicationRepository jobApplicationRepository;
        private readonly IApplicantTaskRepository applicantTaskRepository;
        private readonly IMeetingRepository meetingRepository;
        private readonly IRecruiterRepository recruiterRepository;
        private readonly ICompanyRepository companyRepository;

        public SyncFromJobApplicationToCollections(IJobApplicationRepository _jobApplicationRepository,
        IApplicantTaskRepository _applicantTaskRepository,
        IMeetingRepository _meetingRepository,
        IRecruiterRepository _recruiterRepository,
        ICompanyRepository _companyRepository)
        {
            jobApplicationRepository = _jobApplicationRepository;
            applicantTaskRepository = _applicantTaskRepository;
            meetingRepository = _meetingRepository;
            recruiterRepository = _recruiterRepository;
            companyRepository = _companyRepository;
        }

        [FunctionName("SyncFromJobApplicationToCollections")]
        public static void Run([CosmosDBTrigger(
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

                //companyRepository.
                //jobApplication.Company
                
                //jobApplicationRepository.UpdateItemAsync()
                //applicantTaskRepository.GetItemAsync();
            }
        }
    }
}
