using System;
using System.Collections.Generic;
using MongoDB.Bson;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Enums;
using Xunit;

namespace RN_Process.Tests.DataAccessTests
{
    public class FileImportTest : IDisposable
    {
        public void Dispose()
        {
            _Sut = null;
        }

        public FileImport _Sut;

        public FileImport SystemUnderTest => _Sut ??= _Sut = UnitTestUtility.GetOneFileImportToTest();


        public FileImport InitializeTest(string fileDescription,
            int fileSize,
            string fileFormat,
            string fileLocationOrigin,
            string locationToCopy,
            StatusType status,
            bool fileMigrated,
            DateTime? fileMigratedOn,
            TermDetailConfig termDetailConfig,
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
                termDetailConfig,
                allDataInFile);
        }



        [Fact]
        public void FileImportTestIsCreated_IsValid()
        {
            // Arrange
            // Act
            //Assert
            Assert.NotNull(SystemUnderTest);
            Assert.NotNull(SystemUnderTest.Id);
            Assert.IsType<string>(SystemUnderTest.Id);

            Assert.Equal("file1", SystemUnderTest.FileDescription);
            Assert.Equal(21233, SystemUnderTest.FileSize);
            Assert.Equal("csv", SystemUnderTest.FileFormat);
            Assert.Equal("abc/local/path", SystemUnderTest.FileLocationOrigin);
            Assert.Equal("retorno/copy/to", SystemUnderTest.LocationToCopy);
            Assert.Equal(StatusType.Processed, SystemUnderTest.Status);
            Assert.False(SystemUnderTest.FileMigrated);
            Assert.Null(SystemUnderTest.FileMigratedOn);

            Assert.NotNull(SystemUnderTest.TermDetailConfig);
            Assert.NotNull(SystemUnderTest.TermDetailConfigId);
            Assert.NotEmpty(SystemUnderTest.TermDetailConfigId);

            Assert.Null(SystemUnderTest.ModifiedBy);
            Assert.Null(SystemUnderTest.ModifiedDate);
            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);
            Assert.NotNull(SystemUnderTest.RowVersion);
        }

        //[Fact]
        //public void FileImportTestIsCreated_ThrowException_WhenAllDataInFileHasCorrectType()
        //{
        //    // Arrange
        //    // Act
        //    var ex =
        //        InitializeTest("fq", 1, "csv",
        //            string.Empty,
        //            string.Empty,
        //            StatusType.Processed,
        //            false,
        //            DateTime.UtcNow,
        //            UnitTestUtility.GetTermDetailConfigToTest(),
        //            new List<BsonDocument>());
        //    // Assert
        //    Assert.Equal(BsonType.Document, ex.AllDataInFile.BsonType);
        //}

        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenAllDataInFileIsNull()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
                InitializeTest("teste", 1, "csv",
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    null,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    null));
            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'allDataInFile')", ex.Message);
            Assert.Equal("allDataInFile", ex.ParamName);
        }

        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenFileDescriptionIsEmpty()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                InitializeTest(string.Empty, 1, string.Empty,
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    null,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    new List<BsonDocument>()));
            // Assert
            Assert.Equal("Required input 'FILEDESCRIPTION' was empty. (Parameter 'fileDescription')", ex.Message);
            Assert.Equal("fileDescription", ex.ParamName);
        }


        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenFileDescriptionIsNull()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
                InitializeTest(null, 1, string.Empty,
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    null,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    new List<BsonDocument>()));
            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'fileDescription')", ex.Message);
            Assert.Equal("fileDescription", ex.ParamName);
        }

        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenFileDescriptionIsWhiteSpace()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                InitializeTest(" ", 1, string.Empty,
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    null,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    new List<BsonDocument>()));
            // Assert
            Assert.Equal("Required input 'FILEDESCRIPTION' was empty. (Parameter 'fileDescription')", ex.Message);
            Assert.Equal("fileDescription", ex.ParamName);
        }

        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenFileFormatIsEmpty()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                InitializeTest("fileteste", 1, string.Empty,
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    null,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    new List<BsonDocument>()));
            // Assert
            Assert.Equal("Required input 'FILEFORMAT' was empty. (Parameter 'fileFormat')", ex.Message);
            Assert.Equal("fileFormat", ex.ParamName);
        }


        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenFileFormatIsNull()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
                InitializeTest(null, 1, null,
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    null,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    new List<BsonDocument>()));
            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'fileDescription')", ex.Message);
            Assert.Equal("fileDescription", ex.ParamName);
        }

        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenFileFormatIsWhiteSpace()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                InitializeTest("teste", 1, " ",
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    null,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    new List<BsonDocument>()));
            // Assert
            Assert.Equal("Required input 'FILEFORMAT' was empty. (Parameter 'fileFormat')", ex.Message);
            Assert.Equal("fileFormat", ex.ParamName);
        }

        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenFileMigratedOnIsNotNull()
        {
            // Arrange
            // Act
            var ex =
                InitializeTest("fq", 1, "csv",
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    DateTime.UtcNow,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    new List<BsonDocument>());
            // Assert
            Assert.Equal(DateTime.UtcNow.Date, ex.FileMigratedOn);
        }

        [Fact]
        public void FileImportTestIsCreated_ThrowException_WhenFileSizeIsZero()
        {
            // Arrange
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                InitializeTest("file1", 0, string.Empty,
                    string.Empty,
                    string.Empty,
                    StatusType.Processed,
                    false,
                    null,
                    UnitTestUtility.GetTermDetailConfigToTest(),
                    new List<BsonDocument>()));
            // Assert
            Assert.Equal("Required input 'FILESIZE' cannot be zero. (Parameter 'fileSize')", ex.Message);
            Assert.Equal("fileSize", ex.ParamName);
        }



    }
}