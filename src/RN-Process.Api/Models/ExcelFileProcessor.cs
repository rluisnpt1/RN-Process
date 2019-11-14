using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace RN_Process.Api.Models
{
    public class ExcelFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public ExcelFileProcessor(string inputFilePath, string outputFilePath)
            : this(inputFilePath, outputFilePath, new FileSystem()) { }

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public ExcelFileProcessor(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public void Process()
        {
            throw new NotImplementedException();
        }
    }
}
