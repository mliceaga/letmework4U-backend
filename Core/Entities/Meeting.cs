using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Meeting : BaseEntity
    {
        public string YearPartitionKey { get; set; }
        public string Title { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string MeetingId { get; set; }
        public string MeetingAccessCode { get; set; }
        public Recruiter[] Recruiters { get; set; }
        public Applicant Applicant { get; set; }
        public Offer Offer { get; set; }
        public string Outcome { get; set; }
        public Meeting FollowUpMeeting { get; set; }
        
    }
}
