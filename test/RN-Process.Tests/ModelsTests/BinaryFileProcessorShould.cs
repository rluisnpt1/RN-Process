﻿using System.IO.Abstractions.TestingHelpers;
using RN_Process.Api.Models;
using RN_Process.Shared.FileWork;
using Xunit;

namespace RN_Process.Tests.ModelsTests
{
    public class BinaryFileProcessorShould
    {
        [Fact]
        public void AddLargestNumber()
        {
            var mockInputFile = new MockFileData(new byte[] { 0xFF, 0x34, 0x56, 0xD2 });
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(@"c:\root\in\myfile.data", mockInputFile);
            mockFileSystem.AddDirectory(@"c:\root\out");

            var sut = new BinaryFileProcessor(@"c:\root\in\myfile.data",
                                              @"c:\root\out\myfile.data",
                                              mockFileSystem);
            sut.Process();

            Assert.True(mockFileSystem.FileExists(@"c:\root\out\myfile.data"));

            var processedFile = mockFileSystem.GetFile(@"c:\root\out\myfile.data");

            byte[] data = processedFile.Contents;

            Assert.Equal(5, data.Length);
            Assert.Equal(0xFF, data[4]);
        }
    }
}