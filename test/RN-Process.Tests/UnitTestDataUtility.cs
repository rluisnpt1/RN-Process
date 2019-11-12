using System;

namespace RN_Process.Tests
{
    public static class UnitTestDataUtility
    {
        public static string GetUniqueValue(string prefix)
        {
            var fullValue = $"{prefix}{Guid.NewGuid()}";

            return fullValue.Length > 20 ? fullValue.Substring(0, 20) : fullValue;
        }
    }
}