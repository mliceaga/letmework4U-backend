using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Contact[] Contacts { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
    }
}
