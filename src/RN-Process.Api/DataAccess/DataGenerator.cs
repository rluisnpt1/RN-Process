using System;
using System.Collections.Generic;
using System.Linq;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess
{
    public static class DataGenerator
    {
        public static void Initialize(RnProcessContext context)
        {
            // Look for any board games.
            if (context.Organizations.Any()) return; // Data was already seeded

            if (context.Contracts.Any()) return; // Data was already seeded

            if (context.ContractDetailConfigs.Any()) return; // Data was already seeded

            var listCustomer = new List<Organization>
            {
                new Organization("Banco BBVA", "54TG"),
                new Organization("Banco Portugal", "12DD"),
                new Organization("Banco Banco Do Brasil", "123FF"),
                new Organization("Banco Santander", "4543DSS")
            };

            var listContract = new List<Contract>
            {
                new Contract(445585, listCustomer.FirstOrDefault()),
                new Contract(21224, listCustomer.FirstOrDefault(X => X.OrgCode.Equals("BBPPT")))
            };

            context.Organizations.AddRange(listCustomer);
            context.Contracts.AddRange(listContract);
            //context.ContractDetailConfigs.AddRange(
            //    new ContractDetailConfig(listContract.FirstOrDefault(), FileAccessType.FTP, "LocalHost", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "FTP:10.80.5.198", "ETL", "FTP", true, "MYLogin@MyName", "MyPass1234", null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Origin", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Destination", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Backup", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BackupToHost", ",", new List<string>
            //    {
            //        "NDIV", "COD_CRED", "VAL1", "VAL2",
            //        "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"
            //    }, new List<string>
            //    {
            //        "NDIV", "COD_CRED", "VAL5", "VAL2",
            //        "VAL39", "VAL24", "VAL8", "DATA1", "DATA2", "DATA93"
            //    })
            //);
            context.SaveChanges();
        }
    }
}