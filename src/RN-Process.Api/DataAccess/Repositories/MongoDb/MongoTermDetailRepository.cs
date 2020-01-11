using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess.Repositories.MongoDb
{
    // Implementação
    public class MongoTermDetailRepository : BaseRepositoryMongo<TermDetail>, IRepositoryMongo<TermDetail>
    {
        public MongoTermDetailRepository(IMongoContext context) : base(context)
        {
        }

    }

}
