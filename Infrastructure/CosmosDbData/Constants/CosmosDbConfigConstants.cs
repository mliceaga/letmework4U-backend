using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.CosmosDbData.Constants
{
    public class CosmosDbContainerConstants
    {
        // TODO : consider retrieving this from appsettings using IOptions, instead of defining it as a constant
        public const string CONTAINER_NAME_TODO = "timeslot";
    }
}
