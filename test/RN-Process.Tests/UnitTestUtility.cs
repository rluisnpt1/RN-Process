using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FluentAssertions;
using MongoDB.Bson;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Tests
{
    public static class UnitTestUtility
    {
        //Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        private const string BaseWorkdir = "C:\\TEMP\\WorkDir";

        public static Organization GetBancoPortugalOrganizationToTest()
        {
            return new Organization(string.Empty, "Banco de portugal", "120@1");
        }

        public static Organization GetNOWO_Organization_OrganizationToTest()
        {
            return new Organization(string.Empty, "NOWO ORGANIZATION", "CB13");
        }

        public static Organization GetUNICRE_OrganizationToTest()
        {
            return new Organization(string.Empty, "UNICRE PORTO", "UNPOR");
        }

        public static Term GetContracuNICREtOrganizationToTest()
        {
            return new Term(14533686, GetUNICRE_OrganizationToTest());
        }

        public static Term GetTermOrganizationToTest()
        {
            return new Term(14533686, GetNOWO_Organization_OrganizationToTest());
        }

        public static TermDetail GetTermBaseToTeste()
        {
            return new TermDetail(54856, TermsType.Loan, GetTermOrganizationToTest());
        }

        public static OrganizationFile GetOneFileImportToTest()
        {
            return new OrganizationFile("", "file1", 21233
                , "csv",
                "abc/local/path",
                "retorno/copy/to",
                StatusType.Processed,
                false,
                null, GetTermDetailConfigToTest(),
                new List<BsonDocument>());
        }

        public static TermDetailConfig GetTermDetailConfigToTest(
            TermDetailConfig getTermDetailConfigToTest)
        {
            return getTermDetailConfigToTest;
        }


        public static TermDetailConfig GetTermDetailConfigToTest()
        {
            return new TermDetailConfig(null,
                GetTermBaseToTeste(), FileAccessType.FTP, "LocalHost", BaseWorkdir,
                "ETL", "FTP", true, "MYLogin@MyName", "MyPass1234", "", null,
                BaseWorkdir + "\\backup\\SimulationCliente\\to_intrum",
                RnProcessConstant.BaseTestWorkFolder + "\\Destination",
                RnProcessConstant.BaseTestWorkFolder + "\\Backup",
                BaseWorkdir + "\\BackupToHost",
                ",",
                true,
                string.Empty,
                new List<string>
                    {"NDIV", "COD_CRED", "VAL1", "VAL2", "DATA3"},
                new List<string>
                    {"NDIV", "COD_CRED", "VAL1", "VAL2", "DATA3"});
        }


        public static TermDetailConfig GetRealTermUnicreDetailConfigToTest()
        {
            return new TermDetailConfig("", GetTermBaseToTeste(),
                FileAccessType.FTP,
                "LocalHost",
                "ftp.unicre.pt",
                "SFTP", "SFTP",
                true, "logi", "ih3bb6",
                "ssh-rsa 2048 4e:fd:4f:a3:e3:68:7e:f0:53:91:0d:8d:5f:17:f3:d5",
                null,
                "/De Unicre",
                "/Para Unicre",
                "",
                "", ",", true, string.Empty,
                new List<string>
                    {"NDIV", "COD_CRED", "VAL1"},
                new List<string>
                    {"NDIV", "COD_CRED", "VAL1"});
        }

        public static TermDetailConfig GetRealTermDetailConfigWebSiteToTest()
        {
            return new TermDetailConfig("", GetTermBaseToTeste(),
                FileAccessType.WebSite,
                "LocalHost",
                "https://assist.healthcare.com.pt/index.php?r=auth%2Flogin",
                "AUTH", "INTERNAL RESPONSE", true, "intrum", "int2019#u", "", null,
                "https://assist.healthcare.com.pt/index.php?r=intrum%2Fdebts&export=csv", "", "",
                BaseWorkdir + "\\BackupToHost", ",", true, string.Empty,
                new List<string>
                    {"NDIV", "COD_CRED", "DATA2", "DATA3"},
                new List<string>
                    {"NDIV", "COD_CRED", "DATA2", "DATA3"});
        }


        public static TermDetailConfig TermDetailIdNull()
        {
            return new TermDetailConfig(null, GetTermBaseToTeste(), FileAccessType.LocalMachine,
                string.Empty, "C://Temp", string.Empty,
                string.Empty, false, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, true, string.Empty,
                new List<string> { "" }, new List<string> { "" });
        }

        public static Organization GetCompleteOrganization()
        {
            var info = new Organization(string.Empty, "Banco de Portual", "BP234");

            var detailConfig = TermDetailIdNull();
            info.AddTerm(null, 1423123, 45632, TermsType.Leasing,
                detailConfig.CommunicationType,
                detailConfig.InternalHost,
                detailConfig.LinkToAccess,
                detailConfig.LinkToAccessType,
                detailConfig.TypeOfResponse,
                detailConfig.RequiredLogin,
                detailConfig.AuthenticationLogin,
                Encoding.ASCII.GetString(detailConfig.AuthenticationPassword),
                Encoding.ASCII.GetString(detailConfig.HostKeyFingerPrint),
                detailConfig.AuthenticationCodeApp,
                detailConfig.PathToOriginFile,
                detailConfig.PathToDestinationFile,
                detailConfig.PathToFileBackupAtClient,
                detailConfig.PathToFileBackupAtHostServer,
                detailConfig.FileDelimiter,
                detailConfig.HasHeader,
                detailConfig.FileProtectedPassword,
                detailConfig.FileHeaderColumns,
                detailConfig.AvailableFieldsColumns
            );
            return info;
        }

        public static ContractOrganization GetContractOrganizationModel()
        {
            var infoModel = new ContractOrganization
            {
                Description = "English Bank in Some where",
                CodOrg = "BBEE",
                ContractNumber = 44552368
            };

            infoModel.AddDueDetail(556698, "Leasing");

            var dueDetails = infoModel.DueDetails.Select(x => x);
            foreach (var dueDetail in dueDetails)
                dueDetail.AddDueDetailConfigs(null, "FTP",
                    "SFTP://ftp.unicre.pt",
                    "C://Desktop",
                    "",
                    true,
                    "logi",
                    "ih3bb6",
                    "",
                    null,
                    BaseWorkdir,
                    "SFTP",
                    "FTP",
                    ",",
                    false,
                    string.Empty,
                    new List<string> { "NDIV", "COD_CRED", "VAL1" },
                    RnProcessConstant.AvailableColumnsIntrum
                );

            return infoModel;
        }

        public static ContractOrganization GetContractOrganizationModelTwo()
        {
            var infoModel = new ContractOrganization
            {
                Description = "Banco da China",
                CodOrg = "BP234",
                ContractNumber = 44552368
            };
            infoModel.AddDueDetail(556698, "Leasing");

            var dueDetails = infoModel.DueDetails.Select(x => x);
            foreach (var dueDetail in dueDetails)
                dueDetail.AddDueDetailConfigs(null, "FTP",
                    "SFTP://ftp.unicre.pt",
                    "C://Desktop",
                    "",
                    true,
                    "logi",
                    "ih3bb6",
                    "",
                    null,
                    BaseWorkdir,
                    "SFTP",
                    "FTP",
                    ",",
                    true,
                    string.Empty,
                    new List<string> { "NDIV", "COD_CRED", "VAL1" },
                    RnProcessConstant.AvailableColumnsIntrum
                );

            return infoModel;
        }

        public static FileDataContract GetFileDataContract(string codOrg)
        {
            var dataContract = new FileDataContract(null, codOrg, "firstFile.xml", 555,
                "xml", "c:/temp/teste", "y:/copy/file",
               "Processed", false, null, new List<BsonDocument>()
                );

            return dataContract;
        }

        /// <summary>
        ///     Date Time validate
        /// </summary>
        /// <param name="expectedDate"></param>
        /// <param name="actualDate"></param>
        /// <param name="maximumDelta"></param>
        public static void DateTimeAssertAreEqual(DateTime? expectedDate, DateTime? actualDate, TimeSpan maximumDelta)
        {
            switch (expectedDate)
            {
                case null when actualDate == null:
                    return;
                case null:
                    throw new NullReferenceException("The expected date was null");
                default:
                    {
                        if (actualDate == null) throw new NullReferenceException("The actual date was null");

                        break;
                    }
            }

            var totalSecondsDifference = Math.Abs(((DateTime)actualDate - (DateTime)expectedDate).TotalSeconds);

            if (totalSecondsDifference > maximumDelta.TotalSeconds)
                throw new Exception(
                    $"Expected Date: {expectedDate}, Actual Date: {actualDate} \nExpected Delta: {maximumDelta}, Actual Delta in seconds- {totalSecondsDifference}");
        }

        /// <summary>
        ///     From Entity to Model
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AssertAreEqual(Organization expected, ContractOrganization actual)
        {
            expected.Id.Should().NotBeNullOrEmpty();
            expected.Id.Should().BeEquivalentTo(actual.Id);
            expected.OrgCode.Should().BeEquivalentTo(actual.CodOrg);
            expected.Description.Should().BeEquivalentTo(actual.Description);


            expected.Terms.Should().NotBeNullOrEmpty();
            expected.Terms.Select(x => x.Id).Should().BeEquivalentTo(actual.DueId);
            expected.Terms.Select(x => x.TermNumber).Should().BeEquivalentTo(actual.ContractNumber);

            expected.Terms.Select(x => x.TermDetails).Should().NotBeNullOrEmpty();
            expected.Terms.Select(x => x.TermDetails).Should().HaveCount(actual.DueDetails.Count);
            expected.Terms.Select(x => x.TermDetails.Count.Should().Be(1));
            expected.Terms.Select(x => x.TermDetails.Select(s => s.Id).Should().BeEquivalentTo(actual.DueDetails.Select(z => z.Id)));
            expected.Terms.Select(x => x.TermDetails.Select(s => s.DebtCode).Should()
                .BeEquivalentTo(actual.DueDetails.Select(x => x.DebtCode)));
            expected.Terms.Select(x => x.TermDetails.Select(x => x.TermsType).Should()
                .BeEquivalentTo(actual.DueDetails.Select(x => x.TermsType)));


            expected.Terms.Select(x => x.TermDetails.Select(x => x.TermDetailConfigs).Should().NotBeNullOrEmpty());
            expected.Terms.Select(x => x.TermDetails.Select(x => x.TermDetailConfigs).Should()
                .HaveCount(actual.DueDetails.Select(x => x.DueDetailConfigs.Count).Count()));

            //DateTimeAssertAreEqual(expected.CreatedDate, actual.CreatedDate, TimeSpan.FromMinutes(0.1));
            //DateTimeAssertAreEqual(expected.UpdatedDate, actual.ChangedDate, TimeSpan.FromMinutes(0.1));

            //expected.ModifiedBy.Should().BeEquivalentTo(actual.UpdateBy);
            //expected.CreatedBy.Should().BeEquivalentTo(actual.CreatedBy);
            //expected.Active.Equals(!actual.IsDeleted).Should().BeTrue();
        }

        /// <summary>
        ///     From Model To Entity
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AssertAreEqual(ContractOrganization expected, Organization actual)
        {
            //model side is null actual not null
            expected.Id.Should().BeNullOrEmpty();

            actual.Should().NotBeNull();
            actual.Id.Should().NotBeNullOrEmpty();
            actual.Terms.Select(x => x.Id).Should().NotBeNullOrEmpty();
            actual.Terms.Select(x => x.TermDetails.Select(x => x.Id)).Should().NotBeNullOrEmpty();

            expected.CodOrg.Should().BeEquivalentTo(actual.OrgCode);
            expected.Description.Should().BeEquivalentTo(actual.Description);

            expected.DueDetails.Should().NotBeNullOrEmpty();
            //  expected.DueDetails.Should().HaveCount(actual.Terms.Count);
            //expected.DueDetails.Select(x => x.DueDetailConfigs).Should()
            //    .HaveCount(actual.TermDetails.Select(x => x.TermDetailConfigs.Count).Max());

            ////model side is null actual not null
            expected.DueId.Should().BeNullOrEmpty();

            //verify data in terms
            expected.ContractNumber.Should().BeGreaterThan(0);
            expected.ContractNumber.Should().Be(actual.Terms.Select(x => x.TermNumber).FirstOrDefault());
            expected.CodOrg.Should().Be(actual.Terms.Select(x => x.OrgCode).FirstOrDefault());


            DateTimeAssertAreEqual(DateTime.UtcNow, actual.CreatedDate, TimeSpan.FromMinutes(0.1));
            actual.Active.Should().BeTrue();
            actual.Deleted.Should().BeFalse();
            actual.CreatedBy.Should().NotBeNullOrEmpty();
            actual.UpdatedDate.Should().BeNull();
            actual.ModifiedBy.Should().BeNull();
        }
    }
}