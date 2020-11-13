using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public enum JobApplicationStatus
    {
        NotApplied = 1,
        Applied = 2,
        FirstInterview = 3,
        Meetings = 4,
        Rejected = 5,
        Offer = 6,
        Completed = 7
    }

    public class JobApplication : BaseEntity
    {
        public Company Company { get; set; }
        public string JobTitle { get; set; }
        public string Source { get; set; }
        public string URL { get; set; }
        public string JobDescription { get; set; }
        public decimal Salary { get; set; }
        public string Location { get; set; }
        public bool IsDirect { get; set; }
        public bool IsConsultancy { get; set; }
        public JobApplicationStatus Status { get; set; }
        public DateTime DateApplied { get; set; }
        public string[] RecruiterIds { get; set; }
        public Meeting FirstMeeting { get; set; }
        public Note[] Notes { get; set; }
        public Task[] Tasks { get; set; }
        public Label[] Labels { get; set; }
        public DateTime DeadLine { get; set; }
        public string JobOffer { get; set; }
        public DateTime CreatedOnDate { get; set; }
        // TODO nice to have, an image URL to display the company logo or something like that
        public bool IsDeleted { get; set; }
    }
}
