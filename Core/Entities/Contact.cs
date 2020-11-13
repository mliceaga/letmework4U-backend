using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Contact : BaseEntity
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Cellphone { get; set; }
        public string Landline { get; set; }
        public string ContactEmail { get; set; }
        public string SkypeId { get; set; }
        public string GoogleMeetId { get; set; }
        public Label[] Labels { get; set; }
        public bool IsDeleted { get; set; }
    }
}
