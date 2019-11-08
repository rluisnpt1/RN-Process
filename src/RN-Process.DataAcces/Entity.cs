using System;
using System.ComponentModel.DataAnnotations;
using RN_Process.DataAcces;

namespace RN_Process.DataAccess
{
    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        public T Id { get; private set; }
        

        /// <summary>
        /// The Name value is a string that can store the a string that can store the description .
        /// </summary>
        [StringLength(250)]
        public string Description { get; set; }


        private DateTime? _createdDate;

        object IEntityBase.Id
        {
            get => Id;
            set => Id = (T) value;
        }

        /// <summary>
        ///  The CreatedDate value is a DateTime that can store the complete date of criation.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate
        {
            get => _createdDate ?? DateTime.UtcNow;
            set => _createdDate = value;
        }

        /// <summary>
        /// The ModifiedDate value is a DateTime that can store the complete date of change.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// The CreatedBy value is a string that can store the description name of an user or application
        /// </summary>
        [StringLength(250)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// The ModifiedBy value is a string that can store the description name of an user or application
        /// </summary>
        [StringLength(250)]
        public string ModifiedBy { get; set; }


        /// <summary>
        /// The rowversion value is a sequential number that's incremented each time the row is updated. 
        /// In an Update or Delete command, the Where clause includes the original value of the tracking 
        /// column (the original row version). If the row being updated has been changed by another user, 
        /// the value in the rowversion column is different than the original value, so the Update or 
        /// Delete statement can't find the row to update because of the Where clause. When the Entity 
        /// Framework finds that no rows have been updated by the Update or Delete command (that is, when 
        /// the number of affected rows is zero), it interprets that as a concurrency conflict.
        /// </summary>
        /// [ConcurrencyCheck]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
