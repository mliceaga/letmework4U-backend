using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Interfaces.Persistence;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionsSyncCollections
{
    public class SyncFromRecruiterToCollections
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IApplicantTaskRepository _applicantTaskRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly ICompanyRepository _companyRepository;

        public SyncFromRecruiterToCollections(IJobApplicationRepository jobApplicationRepository,
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

        [FunctionName("SyncFromRecruiterToCollections")]
        public void Run([CosmosDBTrigger(
            databaseName: "letmeworkCosmosDB",
            collectionName: "recruiter",
            ConnectionStringSetting = "letmework4U_DOCUMENTDB",
            LeaseCollectionName = "leasesJobApplications",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                try
                { 
                    var jobApplication = JsonConvert.DeserializeObject<JobApplication>(input[0].ToString());

                    if(jobApplication.Company != null)
                        _companyRepository.UpdateItemAsync(new Guid(jobApplication.Company.Id), jobApplication.Company);

                    if(jobApplication.FirstMeeting != null)
                        _meetingRepository.UpdateItemAsync(new Guid(jobApplication.ApplicantId), jobApplication.FirstMeeting);

                    if(jobApplication.ApplicantTasks.Any())
                    { 
                        foreach (var appTask in jobApplication.ApplicantTasks)
                        {
                            _applicantTaskRepository.UpdateItemAsync(new Guid(jobApplication.ApplicantId), appTask);
                        }
                    }

                    if (jobApplication.Recruiters.Any())
                    { 
                        foreach (var recruiter in jobApplication.Recruiters)
                        {
                            _recruiterRepository.UpdateItemAsync(new Guid(recruiter.Id), recruiter);
                        }
                    }
                }
                catch(Exception ex)
                {
                    // TODO log exception
                    throw ex;
                }

            }
        }
    }
}
