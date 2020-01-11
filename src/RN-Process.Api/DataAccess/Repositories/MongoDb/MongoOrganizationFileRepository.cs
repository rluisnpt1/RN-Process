using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess.Repositories.MongoDb
{
    public class MongoOrganizationFileRepository : BaseRepositoryMongo<OrganizationFile>, IRepositoryMongo<OrganizationFile>
    {
        public MongoOrganizationFileRepository(IMongoContext context) : base(context)
        {
        }

    }
}
