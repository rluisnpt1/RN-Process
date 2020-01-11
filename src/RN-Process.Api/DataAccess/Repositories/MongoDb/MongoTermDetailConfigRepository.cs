using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess.Repositories.MongoDb
{
    // Implementação
    public class MongoTermDetailConfigRepository : BaseRepositoryMongo<TermDetailConfig>, IRepositoryMongo<TermDetailConfig>
    {
        public MongoTermDetailConfigRepository(IMongoContext context) : base(context)
        {
        }


    }

}
