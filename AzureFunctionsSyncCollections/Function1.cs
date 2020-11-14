using System;
using System.Collections.Generic;
using Core.Entities;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSyncCollections
{
    public static class Function1
    {
        [FunctionName("SyncFromJobApplicationToCollections")]
        public static void Run([CosmosDBTrigger(
            databaseName: "letmeworkCosmosDB",
            collectionName: "jobApplications",
            ConnectionStringSetting = "ConnectionStrings:CosmosDB",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("jobApplications modified " + input.Count);
                log.LogInformation("First jobApplication Id " + input[0].Id);
                var jobApplication = (JobApplication)input;
            }
        }
    }
}
