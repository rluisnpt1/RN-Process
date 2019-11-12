using System;
using System.Collections.Generic;
using System.Linq;
using RN_Process.Api.DataAccess.Entities;

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
                new Organization("Banco BBVA", "BBVA112"),
                new Organization("Banco Portugal", "BBPPT"),
                new Organization("Banco Banco Do Brasil", "BDB1s2"),
                new Organization("Banco Santander", "BsA32")
            };

            var listContract = new List<Contract>
            {
                new Contract(445585, 458, "CONSUMO", listCustomer.FirstOrDefault()),
                new Contract(21224, 458, "CONSUMO", listCustomer.FirstOrDefault(X => X.UniqCode.Equals("BBPPT")))
            };

            context.Organizations.AddRange(listCustomer);
            context.Contracts.AddRange(listContract);
            context.ContractDetailConfigs.AddRange(
                new ContractDetailConfig(
                    "FTP",
                    "LocalHost",
                    "FTP:10.80.5.198",
                    "ETL",
                    "FTP",
                    true,
                    "MYLogin@MyName",
                    "MyPass1234",
                    null,
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Origin",
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Destination",
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Backup",
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BackupToHost",
                    ",",
                    new List<string>
                        {"NDIV", "COD_CRED", "VAL1", "VAL2", "VAL3", "VAL4", "VAL5", "DATA1", "DATA2", "DATA3"},
                    listContract.FirstOrDefault()
                )
            );
            context.SaveChanges();
        }
    }
}