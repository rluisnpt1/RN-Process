using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess.Repositories.MongoDb
{
    // Implementação
    public class MongoOrganizationRepository : BaseRepositoryMongo<Organization>, IRepositoryMongo<Organization>
    {
        public MongoOrganizationRepository(IMongoContext context) : base(context)
        {
        }
    }

}
