using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Meeting : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsPhonecall { get; set; }
        public string OnlineMeetingURL { get; set; }
        public string PhysicalLocation { get; set; }
        public string MeetingAccessCode { get; set; }
        public Recruiter[] Recruiters { get; set; }
        public string ApplicantId { get; set; }
        public string JobApplicationId { get; set; }
        public string Outcome { get; set; }
        public Meeting[] FollowUpMeetings { get; set; }
        public string Note { get; set; }
        public string[] Labels { get; set; }
        public bool IsDeleted { get; set; }
    }
}
