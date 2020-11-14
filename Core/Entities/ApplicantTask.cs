using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public enum TaskType
    {
        FollowUp = 1,
        ReachOut = 2,
        SendAvailability = 3,
        PrepareCoverLetter = 4,
        PrepareResume =  5,
        PrepareCVDetailed = 6,
        PrepareForInterview = 7,
        Apply = 8
    }

    public class ApplicantTask: BaseEntity
    {
        public string ApplicantId { get; set; }
        public TaskType Type { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public bool Completed { get; set; }
    }
}
