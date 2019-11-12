using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using RN_Process.Api.DataAccess.Entities;

namespace RN_Process.Api.DataAccess.Persistences
{
    public static class MongoDbMap
    {
        public static void OrganizationConfigure()
        {
            BsonClassMap.RegisterClassMap<Organization>(map =>
            {
              
                map.SetIsRootClass(true);
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.AddKnownType(typeof(Organization));

                map.MapIdMember(x => x.Id).SetIdGenerator(ObjectIdGenerator.Instance);
                map.MapMember(x => x.Description).SetIsRequired(true);
                map.MapMember(x => x.ModifiedDate).SetIsRequired(true);
            });
        } 
        
        public static void ContractConfigure()
        {
            BsonClassMap.RegisterClassMap<Contract>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id).SetIdGenerator(ObjectIdGenerator.Instance);
                map.MapMember(x => x.ContractNumber).SetIsRequired(true);
            });
        }
        public static void ContractMappingBaseConfigure()
        {
            BsonClassMap.RegisterClassMap<ContractMappingBase>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id).SetIdGenerator(ObjectIdGenerator.Instance);
                map.MapMember(x => x.InternalHost).SetIsRequired(true);
            });
        } 
        public static void FileImPortConfigure()
        {
            BsonClassMap.RegisterClassMap<FileImport>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id).SetIdGenerator(ObjectIdGenerator.Instance);
                map.MapMember(x => x.FileDescription).SetIsRequired(true);
            });
        }
    }
}
