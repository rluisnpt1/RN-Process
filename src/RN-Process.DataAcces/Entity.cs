using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RN_Process.DataAccess
{
    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        /// <summary>
        /// 
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual T Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Deleted { get; set; }

        public virtual bool Active { get; set; }

        private byte[] _rowVersion;
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
        public byte[] RowVersion
        {
            get => _rowVersion ??= _rowVersion = new byte[0];
            set => value = _rowVersion;
        }
    }

}
