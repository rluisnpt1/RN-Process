using System;

namespace RN_Process.DataAccess
{
    public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
    {
        private readonly string _createdBy;
        private DateTime? _createdDate;

        public DateTime CreatedDate
        {
            get => (DateTime) (_createdDate ??= _createdDate = DateTime.UtcNow);
            set
            {
                if (_createdDate != null) value = (DateTime) _createdDate;
            }
        }

        public string CreatedBy
        {
            get => _createdBy ?? "Internal System";
            set { }
        }


        public DateTime? UpdatedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}