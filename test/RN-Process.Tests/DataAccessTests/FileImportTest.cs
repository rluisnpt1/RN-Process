using System;
using System.Collections.Generic;
using MongoDB.Bson;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Enums;

namespace RN_Process.Tests.DataAccessTests
{
    public class FileImportTest : IDisposable
    {
        public FileImport _Sut;

        public FileImport SystemUnderTest
        {
            get
            {
                return _Sut ??= _Sut = new FileImport(
                    "file1", 21233
                    , "csv",
                    "abc/local/path",
                    "retorno/copy/to",
                    StatusType.Processed,
                    false,
                    null, UnitTestUtility.GetContractDetailConfigToTest(),
                    new List<BsonDocument>());
            }
        }

        public void Dispose()
        {
            _Sut = null;
        }

        public FileImport InitializeTest(
            string fileDescription,
            int fileSize,
            string fileFormat,
            string fileLocationOrigin,
            string locationToCopy,
            StatusType status,
            bool fileMigrated,
            DateTime? fileMigratedOn,
            ContractDetailConfig contractDetailConfig,
            List<BsonDocument> allDataInFile)
        {
            return new FileImport(fileDescription,
                fileSize,
                fileFormat,
                fileLocationOrigin,
                locationToCopy,
                status,
                fileMigrated,
                fileMigratedOn,
                contractDetailConfig,
                allDataInFile);
        }
    }
}