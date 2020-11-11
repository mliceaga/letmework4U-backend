using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Offer: BaseEntity
    {
        public string Source { get; set; }
        public string SiteURL { get; set; }
        public string JobDescription { get; set; }
        public bool IsDirect { get; set; }
        public bool IsRecruiter { get; set; }
        public string CompanyId { get; set; }
        public string RecruiterId { get; set; }
    }
}
