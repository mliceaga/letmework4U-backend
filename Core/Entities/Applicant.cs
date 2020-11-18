using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Applicant : Contact
    {
        public string CurrentJob { get; set; }
        public string LinkedInUrl { get; set; }
        public string[] JobApplicationIds { get; set; }
    }
}
