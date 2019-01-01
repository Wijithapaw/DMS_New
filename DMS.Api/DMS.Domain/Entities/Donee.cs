using DMS.Domain.Entities.Base;
using System;

namespace DMS.Domain.Entities
{
    public class Donee : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string  Email { get; set; }

        public bool Active { get; set; }

        public DateTime Birthday { get; set; }
    }
}
