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
        public string MeetingId { get; set; }
        public string MeetingAccessCode { get; set; }
        public string[] RecruiterIds { get; set; }
        public string ApplicantId { get; set; }
        public string JobApplicationId { get; set; }
        public string Outcome { get; set; }
        public Meeting FollowUpMeeting { get; set; }
        public Note Note { get; set; }
        public Label[] Labels { get; set; }
        public bool IsDeleted { get; set; }
    }
}
