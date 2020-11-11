using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class JobApplication: BaseEntity
    {
        public string Source { get; set; }
        public string SiteURL { get; set; }
        public string JobDescription { get; set; }
        public bool IsDirect { get; set; }
        public bool IsRecruiter { get; set; }
        public string CompanyId { get; set; }
        public Recruiter[] Recruiters { get; set; }
    }
}
