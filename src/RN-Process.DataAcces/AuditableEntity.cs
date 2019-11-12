using System;

namespace RN_Process.DataAccess
{
    public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
    {
        private string _createdBy;
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
            set => value = "Internal System";
        }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}