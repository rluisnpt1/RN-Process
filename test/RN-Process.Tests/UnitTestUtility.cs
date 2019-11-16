using System;
using System.Collections.Generic;
using MongoDB.Bson;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Tests
{
    public static class UnitTestUtility
    {
        public static Organization GetBancoPortugalOrganizationToTest()
        {
            return new Organization("Banco de portugal", "120@12");
        }  
        
        public static Organization GetNOWO_Organization_OrganizationToTest()
        {
            return new Organization("NOWO ORGANIZATION", "CB13");
        }
        public static Organization GetUNICRE_OrganizationToTest()
        {
            return new Organization("UNICRE PORTO", "UNPOR");
        }

        public static Term GetContracuNICREtOrganizationToTest()
        {
            return new Term(14533686,  GetUNICRE_OrganizationToTest());
        }  
        
        public static Term GetTermOrganizationToTest()
        {
            return new Term(14533686, GetNOWO_Organization_OrganizationToTest());
        }  
        
        public static TermDetail GetTermBaseToTeste()
        {
            return new TermDetail(54856, TermsType.Loan ,GetTermOrganizationToTest());
        }

        public static FileImport GetOneFileImportToTest()
        {
            return new FileImport(
                "file1", 21233
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

        public static TermDetailConfig GetTermDetailConfigToTest(string codorg)
        {
            return new TermDetailConfig(GetTermBaseToTeste(), FileAccessType.FTP, "LocalHost", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\WorkDir\\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ETL", "FTP", true, "MYLogin@MyName", "MyPass1234", null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\backup\\SimulationCliente\\to_intrum", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\backup\\SimulationCliente\\to_intrum\\Processados\\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\backup\\SimulationCliente\\Backup\\", RnProcessConstant.BaseTestWorkFolder + "\\BackupToHost", ",", new List<string>
                {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"}, new List<string>
                {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"});
        }
        public static TermDetailConfig GetTermDetailConfigToTest()
        {
            return new TermDetailConfig(GetTermBaseToTeste(), FileAccessType.FTP, "LocalHost", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\WorkDir\\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ETL", "FTP", true, "MYLogin@MyName", "MyPass1234", null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\backup\\SimulationCliente\\to_intrum", RnProcessConstant.BaseTestWorkFolder + "\\Destination", RnProcessConstant.BaseTestWorkFolder + "\\Backup", RnProcessConstant.BaseTestWorkFolder + "\\BackupToHost", ",", new List<string>
                {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"}, new List<string>
                {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"});
        }


        public static TermDetailConfig GetRealTermDetailConfigToTest()
        {
            return new TermDetailConfig(GetTermBaseToTeste(), 
                FileAccessType.FTP,
                "LocalHost",
                 "C:\\TEMP",
                 "SFTP://IDCFTPGW.INTRUM.NET:22222",
                 "SFTP", "FTP", true, "nowo", "NX,CD[}?", null,
                 "/to_intrum/",
                 "/from_intrum/",
                "",
                 "", ",", 
                 new List<string>
                     {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"}, new List<string>
                    {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"});
        } 
        public static TermDetailConfig GetRealTermUnicreDetailConfigToTest()
        {
            return new TermDetailConfig(GetTermBaseToTeste(), 
                FileAccessType.FTP,
                "LocalHost",
                 "C:\\TEMP",
                 "SFTP://ftp.unicre.pt",
                 "SFTP", "FTP", true, "logi", "ih3bb6", null,
                 "/Da Unicre",
                 "/Para Unicre",
                "",
                 "", ",", 
                 new List<string>
                     {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"}, new List<string>
                    {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"});
        }
        public static TermDetailConfig GetRealTermDetailConfigWebSiteToTest()
        {
            return new TermDetailConfig(GetTermBaseToTeste(),
                FileAccessType.WebSite,
                "LocalHost",
                "C:\\TEMP\\Workdir",
                "https://assist.healthcare.com.pt/index.php?r=auth%2Flogin",
                "AUTH", "INTERNAL RESPONSE", true, "intrum", "int2019#u", null,
                "https://assist.healthcare.com.pt/index.php?r=intrum%2Fdebts&export=csv", "", "",
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BackupToHost", ",",
                new List<string>
                    {"NDIV", "COD_CRED",  "DATA2", "DATA3"},
                new List<string>
                    {"NDIV", "COD_CRED", "DATA2", "DATA3"});
        }


        public static List<Term> GetManyTermOrganizationToTest()
        {
            var list = new List<Term>();

            var listc = new Term(14533686, GetBancoPortugalOrganizationToTest());
            var listb = new Term(14533686, GetBancoPortugalOrganizationToTest());
            list.Add(listc);
            list.Add(listb);
            return list;
        }

        ///// <summary>
        ///// From Entity to Model
        ///// </summary>
        ///// <param name="expected"></param>
        ///// <param name="actual"></param>
        //public static void AssertAreEqual(ManagerTask expected, ManagerTaskModel actual)
        //{
        //    Assert.Equal(expected.Id, actual.Id);
        //    Assert.Equal<string>(expected.Name, actual.Name);
        //    Assert.Equal<string>(expected.Status, actual.Status);
        //    Assert.Equal<int>(expected.CreationCount, actual.CreationCount);
        //    Assert.Equal<int>(expected.ModificationCount, actual.ModificationCount);
        //    DateTimeAssertAreEqual(expected.TimeStamp, actual.TimeStamp, TimeSpan.FromMinutes(0.1));
        //}

        ///// <summary>
        ///// From Model To Entity
        ///// </summary>
        ///// <param name="expected"></param>
        ///// <param name="actual"></param>
        //public static void AssertAreEqual(ManagerTaskModel expected, ManagerTask actual)
        //{
        //    Assert.Equal(expected.Id, actual.Id);
        //    Assert.Equal(expected.Name, actual.Name);
        //    Assert.Equal(expected.Status, actual.Status);
        //    Assert.Equal<int>(expected.CreationCount, actual.CreationCount);
        //    Assert.Equal<int>(expected.ModificationCount, actual.ModificationCount);
        //    DateTimeAssertAreEqual(expected.TimeStamp, actual.TimeStamp, TimeSpan.FromMinutes(0.1));
        //}

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
    }
}