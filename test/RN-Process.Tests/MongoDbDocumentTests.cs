using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests
{
    public class MongoDbDocumentTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public MongoDbDocumentTests(ITestOutputHelper testOutputHelper)
        {
            JsonWriterSettings.Defaults.Indent = true;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void EmptyDocument()
        {
            var document = new BsonDocument();
            _testOutputHelper.WriteLine(document.ToString());
        }

        [Fact]
        public void AddElement()
        {
            var Organization = new BsonDocument()
            {
                {"contract",new BsonInt32(55) },
                {"valor",new BsonDecimal128(6655) },
                {"Date",new BsonDateTime(DateTime.UtcNow) },
                {"IsDeleted",false }
            };
            Organization.Add("codclient", new BsonString("4455db"));

            _testOutputHelper.WriteLine(Organization.ToString());
        }
        [Fact]
        public void AddArrays()
        {
            var Organization = new BsonDocument();
            Organization.Add("files", new BsonArray(
                new[] { "file.xml", "455", "http://10.05.0.0" }));

            _testOutputHelper.WriteLine(Organization.ToString());
        }

        [Fact]
        public void AddEmbededDocument()
        {
            var Organization = new BsonDocument()
            {
                {"uniqcode",new BsonInt32(1) },
                {"description","Client Name" },
                {
                    "Contract",new BsonDocument()
                    {
                        {"contractnumber",new BsonInt32(65645) },
                        {"typedebt",new BsonInt32(856)},
                        {"debtdescription","Consumo" },

                        {
                            "ContractMappingBase",new BsonDocument()
                            {
                                {"codreference","FTP" },
                                {"internalhost","Http://localhost" },
                                {"linktoaccess","https://1002.10.0.2" },
                                {"linktoaccestipo","http://localhostTest" },
                            }
                        }
                    }

                }
            };

            _testOutputHelper.WriteLine(Organization.ToString());
        }

        [Fact]
        public void BsonValueConversions()
        {
            var Organization = new BsonDocument
            {
                {"contractValue", new BsonInt32(55665)}
            };

            _testOutputHelper.WriteLine(Organization["contractValue"].ToDecimal().ToString());
        }

           [Fact]
        public void ToBson()
        {
            var Organization = new BsonDocument
            {
                {"contractValue", new BsonInt32(55665)}
            };

            var bson = Organization.ToBson();
            var converter = BitConverter.ToString(bson);

            _testOutputHelper.WriteLine(converter);

            var descerilizeOrganization = BsonSerializer.Deserialize<BsonDocument>(bson);
            _testOutputHelper.WriteLine(descerilizeOrganization.ToString());
        }

    }
}
