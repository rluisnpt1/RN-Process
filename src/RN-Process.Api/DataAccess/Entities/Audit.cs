using System;

namespace RN_Process.Api.DataAccess.Entities
{
    /// <summary>
    /// https://www.meziantou.net/entity-framework-core-history-audit-table.htm
    /// https://damienbod.com/2017/02/28/implementing-an-audit-trail-using-asp-net-core-and-elasticsearch-with-nest/
    /// </summary>
    public class Audit
    {
        public Guid Id { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
}
