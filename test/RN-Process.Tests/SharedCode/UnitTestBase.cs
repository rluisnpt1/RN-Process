using System;
using System.Collections.Generic;
using System.Text;
using Moq;

namespace RN_Process.Tests
{
    public class UnitTestBase
    {
        public MockRepository MockRepository { get; private set; }


        public UnitTestBase()
        {
            MockRepository = new MockRepository(MockBehavior.Strict) { DefaultValue = DefaultValue.Empty };
        }

        //public void Dispose()
        //{
        //    MockRepository.VerifyAll();
        //}
    }
}
