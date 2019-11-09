using System;
using System.Collections.Generic;
using RN_Process.Api.Models;
using RN_Process.Shared.Enums;
using Xunit;

namespace RN_Process.Tests
{
    public static class UnitTestUtility
    {
        public static Customer GetBancoPortugalCustomerToTest()
        {
            return new Customer("Banco de portugal", "BDPT48");
        }

        public static Contract GetContractCustomerToTest()
        {
            return new Contract(14533686, 5546, "Divida 1"  , GetBancoPortugalCustomerToTest());
        }

        public static List<Contract> GetManyContractCustomerToTest()
        {
            var list = new List<Contract>();

            var listc = new Contract(14533686,323,"divida 2", GetBancoPortugalCustomerToTest());
            var listb = new Contract(14533686, 65991 ,"Divida 1",GetBancoPortugalCustomerToTest());
            list.Add(listc);
            list.Add(listb);
            return list;
        }

        public static List<Customer> GetManyCustomersToTest()
        {
            var listB = new List<Customer>();
            var t = new Customer("Banco de portugal", "BDPT48");
            var t1 = new Customer("Banco de Lisboa", "BDLI48");
            var t2 = new Customer("Banco de Aveiro", "BDAV78");
            var t3 = new Customer("Banco de Porto", "@BDPP48");
            listB.Add(t);
            listB.Add(t1);
            listB.Add(t2);
            listB.Add(t3);
            return listB;
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
        /// Date Time validate
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
                        if (actualDate == null)
                        {
                            throw new NullReferenceException("The actual date was null");
                        }

                        break;
                    }
            }
            double totalSecondsDifference = Math.Abs(((DateTime)actualDate - (DateTime)expectedDate).TotalSeconds);

            if (totalSecondsDifference > maximumDelta.TotalSeconds)
            {
                throw new Exception(
                    $"Expected Date: {expectedDate}, Actual Date: {actualDate} \nExpected Delta: {maximumDelta}, Actual Delta in seconds- {totalSecondsDifference}");
            }
        }


    }
}

