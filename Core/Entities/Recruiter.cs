using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Recruiter : Contact
    {
        public Company Company { get; set; }
        public bool IsConsultancy { get; set; }
    }
}
