using System;

namespace Core.Domain.Persistence.Common
{
    public class BaseEntity
    {
        public DateTime? CreationDate { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }
    }
}
